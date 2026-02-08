using Game.Data;
using System;
public class AmmoDataImport : DataImportBase
{
    public const string KEY_AMMOAMMOTYPE = "_ammoType";
    public const string KEY_AMMODAMAGE = "_damage";
    public const string KEY_AMMOPENETRATIONPOWER = "_penetrationPower";
    public override void Apply(ItemData asset, string[] fields)
    {
        base.Apply(asset, fields);

        // 2. Gun固有の型にキャストして、追加データを埋める
        if (!(asset is AmmoData ammoData)) return;

        SetField(ammoData, KEY_AMMOAMMOTYPE, fields);
        SetField(ammoData, KEY_AMMODAMAGE, fields);
        SetField(ammoData, KEY_AMMOPENETRATIONPOWER, fields);
    }
}