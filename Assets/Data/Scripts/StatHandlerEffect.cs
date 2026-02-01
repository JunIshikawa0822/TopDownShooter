using System.Collections.Generic;
using UnityEngine;
//**********装備による一時的な値の変化量を管理する**********
public class StatHandlerEffect
{
    private struct CacheEntry
    {
        public float Offset;     // 計算された変化量
        public float BaseValue;  // 計算に使用した基本値
    }

    private class EffectEntry
    {
        public float Duration;
        public readonly IStatModifierProvider Provider;
        public bool IsDiscard => Duration <= 0;
        public EffectEntry(IStatModifierProvider effect, float duration)
        {
            Provider = effect;
            Duration = duration;
        }
    }

    //登録されたEffectを管理
    private readonly Dictionary<string, EffectEntry> _activeEffectProviders = new Dictionary<string, EffectEntry>();

    //キーごとの計算戦略（なければDefaultを使う）
    private readonly Dictionary<string, IStatCalculationStrategy> _strategies = new Dictionary<string, IStatCalculationStrategy>();
    private readonly IStatCalculationStrategy _defaultStrategy = new StatCalculationStrategy_Default();

    //キャッシュした最終的な計算結果
    private readonly Dictionary<string, CacheEntry> _cache = new();
    private readonly HashSet<string> _dirtyStats = new(); //どのステータスが再計算必要か

    //削除用
    private readonly List<string> _removalBuffer = new List<string>();

    //特定のキーに特殊な計算式を割り当てる
    public void SetStrategy(string key, IStatCalculationStrategy strategy)
    {
        _strategies[key] = strategy;
    }

    //毎フレーム呼ばれる
    public void Tick(float deltaTime)
    {
        _removalBuffer.Clear();

        //まず「消すべきもの」を調べる（ここでは削除しない）
        foreach (KeyValuePair<string, EffectEntry> pair in _activeEffectProviders)
        {
            pair.Value.Duration -= deltaTime;
            if (pair.Value.IsDiscard)
            {
                _removalBuffer.Add(pair.Key);
            }
        }

        //調べ終わった後に、まとめて安全に削除する
        foreach (string id in _removalBuffer)
        {
            //RemoveEffect経由で呼ぶことで、キャッシュも正しくクリアされる
            RemoveEffect(id);
        }
    }

    //装備の「補正値」の側面を渡す
    public void AddEffect(StatusEffect statEffect)
    {
        if(statEffect == null) return;
        if (_activeEffectProviders.TryGetValue(statEffect.EffectID, out EffectEntry existing))
        {
            //すでに存在する場合は、効果時間の上書きだけする
            existing.Duration = statEffect.Duration;
        }
        else
        {
            //存在しない場合はキーごと追加する
            EffectEntry newEntry = new EffectEntry(statEffect, statEffect.Duration);
            _activeEffectProviders.Add(statEffect.EffectID, newEntry);

            //パラメータに対してdirtyをセット
            MarkAffectedStatsDirty(newEntry.Provider);
        }
    }

    //装備の「補正値」の側面をはずす
    public void RemoveEffect(string effectID)
    {
        if (_activeEffectProviders.TryGetValue(effectID, out EffectEntry existEntry))
        {
            //パラメータに対してdirtyをセット
            MarkAffectedStatsDirty(existEntry.Provider);

            //EffectEntryを削除
            _activeEffectProviders.Remove(effectID);
        }
    }

    //特定のEffectEntryが影響を与えるステータスのみDirtyフラグを立てる
    private void MarkAffectedStatsDirty(IStatModifierProvider provider)
    {
        foreach (StatModifier mod in provider.GetModifiers())
        {
            _dirtyStats.Add(mod.StatName);
        }
    }

    public float GetOffsetValue(string statName, float baseValue)
    {
        //dirtyでない&statNameで登録されている
        if (!_dirtyStats.Contains(statName) && _cache.TryGetValue(statName, out CacheEntry entry))
        {
            //基本値も変わっていなければそのままキャッシュした値を渡す
            if (Mathf.Approximately(entry.BaseValue, baseValue))
            {
                return entry.Offset;
            }
        }

        //再計算が必要な場合
        //対応したStatModifierを集める
        List<StatModifier> relevantModifiers = new List<StatModifier>();
        foreach (EffectEntry effectEntry in _activeEffectProviders.Values)
        {
            foreach (StatModifier mod in effectEntry.Provider.GetModifiers())
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