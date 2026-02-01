[System.Flags]//プレイヤーができること（Action）
public enum ItemActionTag
{
    None        = 0,
    Flammable   = 1 << 0, // 燃えやすい
    Conductive  = 1 << 1, // 電気を通す
    Explosive   = 1 << 2, // 衝撃や熱で爆発する
    Buoyant     = 1 << 3  // 水に浮く
}