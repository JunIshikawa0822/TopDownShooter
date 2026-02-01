using System.Collections.Generic;
using UnityEngine;
//**********装備による一時的な値の変化量を管理する**********
public class StatHandlerEquipment
{
    private struct CacheEntry
    {
        public float Offset;     // 計算された変化量
        public float BaseValue;  // 計算に使用した基本値
    }
    //登録されたProvider（この場合は「装備」の数値を変更する側面）
    private readonly HashSet<IStatModifierProvider> _activeEquipProviders = new HashSet<IStatModifierProvider>();

    //キーごとの計算戦略（なければDefaultを使う）
    private readonly Dictionary<string, IStatCalculationStrategy> _strategies = new Dictionary<string, IStatCalculationStrategy>();
    private readonly IStatCalculationStrategy _defaultStrategy = new StatCalculationStrategy_Default();

    //キャッシュした最終的な計算結果
    private readonly Dictionary<string, CacheEntry> _cache = new();
    private readonly HashSet<string> _dirtyStats = new(); //どのステータスが再計算必要か

    //特定のキーに特殊な計算式を割り当てる
    public void SetStrategy(string key, IStatCalculationStrategy strategy)
    {
        _strategies[key] = strategy;
    }

    //装備の「補正値」の側面を渡す
    public void AddEquipmentProvider(IStatModifierProvider provider)
    {
        if(provider == null) return;
        if (_activeEquipProviders.Add(provider)) MarkAffectedStatsDirty(provider);
    }

    //装備の「補正値」の側面をはずす
    public void RemoveEquipmentProvider(IStatModifierProvider provider)
    {
        if(provider == null) return;
        if (_activeEquipProviders.Remove(provider)) MarkAffectedStatsDirty(provider);
    }

    //特定のProviderが影響を与えるステータスのみDirtyフラグを立てる
    private void MarkAffectedStatsDirty(IStatModifierProvider provider)
    {
        foreach (StatModifier mod in provider.GetModifiers())
        {
            _dirtyStats.Add(mod.StatName);
        }
    }

    public float GetOffsetValue(string statName, float baseValue)
    {
        // キャッシュをチェック
        if (!_dirtyStats.Contains(statName) && _cache.TryGetValue(statName, out CacheEntry entry))
        {
            // Providerに変更がなく、かつ計算時の基本値も変わっていなければスキップ
            if (Mathf.Approximately(entry.BaseValue, baseValue))
            {
                return entry.Offset;
            }
        }

        //再計算が必要な場合
        List<StatModifier> relevantModifiers = new List<StatModifier>();
        foreach (IStatModifierProvider provider in _activeEquipProviders)
        {
            foreach (StatModifier mod in provider.GetModifiers())
            {
                if (mod.StatName == statName) relevantModifiers.Add(mod);
            }
        }

        //例えばkeyに対応する装備が外された直後の値取得で呼ばれる
        if(relevantModifiers.Count == 0) 
        {
            _cache[statName] = new CacheEntry { Offset = 0, BaseValue = baseValue };
            _dirtyStats.Remove(statName); 
            return 0;
        }

        //戦略を選んで計算
        IStatCalculationStrategy strategy = _strategies.GetValueOrDefault(statName, _defaultStrategy);
        float result = strategy.Calculate(baseValue, relevantModifiers);

        //キャッシュに保存してDirtyフラグを下ろす
        //「今の基本値」も一緒に保存する
        _cache[statName] = new CacheEntry { Offset = result, BaseValue = baseValue };
        _dirtyStats.Remove(statName);

        return result;
    }
}