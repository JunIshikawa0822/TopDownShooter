using System.Collections.Generic;
using System.Linq;

// 制作意図: 複数のアクティブな効果の残り時間を管理し、期限切れの効果を自動で解除する。
// 用途: ゲームのUpdateループから呼び出され、時間管理を担当する。
public class BuffManager
{
    private List<Buff> _activeBuffs = new List<Buff>();
    private Stats _targetStats; // 影響を与えるStatsクラスへの参照

    public BuffManager(Stats stats)
    {
        _targetStats = stats;
    }

    //毎フレーム呼び出され、時間の更新と期限切れ効果の削除を行う。
    public void Update(float deltaTime)
    {
        List<Buff> expiredBuffs = new List<Buff>();
        
        //全てのアクティブな効果の時間を減らす
        foreach (Buff buff in _activeBuffs)
        {
            buff.Duration -= deltaTime;
            if (buff.Duration <= 0)
            {
                expiredBuffs.Add(buff);
            }
        }
        
        //期限切れの効果を解除
        foreach (Buff buff in expiredBuffs)
        {
            RemoveEffect(buff);
        }
    }

    //新しい効果を適用する。
    //BuffManager.ApplyEffect(new Effect("HealBuff", new StatsData { HealthRegen = 5 }, 10f, true));
    public void ApplyBuff(Buff newBuff)
    {
        // === 重複不可ロジックの追加 ===
        // 同じIDを持つ既存の効果を検索 (重複不可な効果のみを対象にする)
        Buff existingBuff = _activeBuffs.FirstOrDefault(e => e.BuffType == newBuff.BuffType);
    
        if (existingBuff != null)
        {
            //既存の効果を完全に置き換える
            RemoveEffect(existingBuff); // 既存効果を解除（Statsから減算）
            _activeBuffs.Remove(existingBuff); // リストから削除
        }

        _activeBuffs.Add(newBuff);
        
        //Statsクラスに効果の適用を依頼
        _targetStats.ApplyEffectToLayer(newBuff.StatsModifier, newBuff.IsBuff, true);
    }

    // 制作意図: 効果をリストから削除し、Statsレイヤーからも減算する。
    public void RemoveEffect(Buff expiredBuff)
    {
        _activeBuffs.Remove(expiredBuff);
        
        //Statsクラスに効果の解除（減算）を依頼
        _targetStats.ApplyEffectToLayer(expiredBuff.StatsModifier, expiredBuff.IsBuff, false);
    }
}