public enum StatusEffectMode
{
    StatOnce,//一度変化し、終了後に戻る（装備によるステータス増減など）
    CurrentInstant,//一度変化し、戻らない（即時ダメージなど）
    StatContinuous,//変化し続け、終了後に戻る（あんまない　SEKIROの老いみたいな）
    CurrentPeriodic//変化し続け、戻らない（出血・毒ダメージ）
}