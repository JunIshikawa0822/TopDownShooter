public enum ItemType
{
    None,
    // 装備
    Weapon,
    Armor,
    Accessory,
    Attachment,//武器カスタマイズ
    Ammo,//弾薬
    Throwable,//投げ物
    Container,//バックパックやポーチ

    // 消耗品
    Medical,//回復
    Food,//食料
    Drink,//飲料
    Stimulant,//バフ・強化

    // 素材・価値
    Material,//クラフト素材
    Valuable,//換金用
    Intel,//情報・機密データ

    // 進行・その他
    KeyItem,//鍵・カードキー
}