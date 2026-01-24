using Game.Data;
using System;
public class AmmoDataImport : DataImportBase
{
    public override void Apply(ItemData asset, string[] fields)
    {
        base.Apply(asset, fields);

        // 2. Gun固有の型にキャストして、追加データを埋める
        if (!(asset is AmmoData ammoData))return;
        
        SetField(ammoData, "_caliberType", fields);
        SetField(ammoData, "_damage", fields);
        SetField(ammoData, "_penetrationPower", fields);
    }
}