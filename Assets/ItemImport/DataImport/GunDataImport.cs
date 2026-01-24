using Game.Data;
using System;
public class GunDataImport : WeaponDataImport
{
    public override void Apply(ItemData asset, string[] fields)
    {
        base.Apply(asset, fields);

        // 2. Gun固有の型にキャストして、追加データを埋める
        if (!(asset is GunData gunData))return;
        
        SetField(gunData, "_fireType", fields);
        SetField(gunData, "_burstCount", fields);
        SetField(gunData, "_fireRate", fields);
        SetField(gunData, "_horizontalRecoil", fields);
        SetField(gunData, "_verticalRecoil", fields);
        SetField(gunData, "_accuracy", fields);
        SetField(gunData, "_bulletVelocity", fields);
        SetField(gunData, "_maxRange", fields);
        SetField(gunData, "_bulletSpawnPos", fields);
    }
}