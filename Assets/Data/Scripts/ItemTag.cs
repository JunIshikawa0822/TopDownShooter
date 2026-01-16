[System.Flags]
public enum ItemTag
{
    None        = 0,
    Tradable    = 1 << 0,//売買可能
    Unique      = 1 << 2,//一点物
    Craftable   = 1 << 3,//クラフト素材になる
    Undiscardable   = 1 << 4,//捨てられない
    QuestRelated = 1 << 5//クエスト関連
}
