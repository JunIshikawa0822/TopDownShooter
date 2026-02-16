using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    public float PenetrateValue;//貫通力
    public float DamageValue;//ダメージ量
    public float ExplodeValue;//爆発力
    public float StanValue;//スタン値
    public float KnockbackValue;//ノックバック量
    public float CriticalChance;//クリティカル率
    public float CriticalDamageMultiplier;//クリティカルダメージ倍率
    public float FixedDamage;//固定ダメージ
    public List<Effect> Effects;//付与する効果
    public bool IgnoreArmor;//防具無視するかどうか
}