// 制作意図: 一時的なステータス変化（バフ/デバフ）一つ一つを定義する。
public class Buff
{
    // 追加: 効果の種別を識別するID
    public BuffType BuffType { get; set; } 
    public StatsData StatsModifier { get; set; } // この効果がもたらすステータス変化
    public float Duration { get; set; }          // 効果の残り時間 (秒)
    public bool IsBuff { get; set; }             // バフ (true) かデバフ (false) か

    // コンストラクタを更新
    public Buff(BuffType buffType, StatsData modifier, float duration, bool isBuff)
    {
        this.BuffType = buffType;
        this.StatsModifier = modifier;
        this.Duration = duration;
        this.IsBuff = isBuff;
    }
}

public enum BuffType
{
    Heal,
    Attack,
    ContinuousDamage,
    HyperContinuousDamage
    
}