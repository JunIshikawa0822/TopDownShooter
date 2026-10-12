[System.Flags]
public enum ItemTraitTag
{

    None = 0,
    Consumable = 1 << 0, // 食べたり飲んだりできる
    Equipable = 1 << 1, // 装備できる
    Loadable = 1 << 2, // 弾丸として銃に込められる
    Throwable = 1 << 3  // 投擲武器として使える
}