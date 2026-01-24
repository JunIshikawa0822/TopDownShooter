using Game.Data;
using System;
public class WeaponDataImport : DataImportBase
{
    public override void Apply(ItemData asset, string[] fields)
    {
        base.Apply(asset, fields);

        // 2. Gun固有の型にキャストして、追加データを埋める
        if (!(asset is WeaponData weapon))return;
        
        SetField(weapon, "_weaponType", fields);
        SetAddressableField(weapon, "_weaponAnimation", fields);
        
        //TODO: 付帯可能エンチャントの変換とScriptableObjectへのセット
        //SetField(weapon, "_attachableEnchants", fields);
    }

    protected override object ConvertValue(Type type, string val)
    {
        //TODO: 付帯可能エンチャントをシートから読み込んで変換する工程
        return base.ConvertValue(type, val);
    }
}