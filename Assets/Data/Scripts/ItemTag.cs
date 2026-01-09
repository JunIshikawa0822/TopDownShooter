[System.Flags]
public enum ItemTag
{
    None        = 0,
    Tradable    = 1 << 0, // 売買可能
    Stackable   = 1 << 1, // スタック可能
    Unique      = 1 << 2, // 一点物
    Craftable   = 1 << 3, // クラフト素材になる
    Droppable   = 1 << 4, // ドロップ可能
    QuestRelated = 1 << 5 // クエスト関連
}
