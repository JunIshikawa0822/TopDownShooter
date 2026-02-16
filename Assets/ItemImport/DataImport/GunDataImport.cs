using Game.Data;
using System;
public class GunDataImport : WeaponDataImport
{
    public const string KEY_TARGETAMMO = "_targetAmmo";
    public const string KEY_ISINTERNALMAGAZINE = "_isInternalMagazine";
    public const string KEY_INTERNALCAPACITY = "_internalCapacity";

    public const string KEY_FIRETYPE = "_fireType";
    public const string KEY_BURSTCOUNT = "_burstCount";
    public const string KEY_SIMULNUM = "_simulNum";
    public const string KEY_RPM = "_rpm";

    public const string KEY_VELOCITY = "_velocity";
    public const string KEY_HORIZONTALRECOIL = "_horizontalRecoil";
    public const string KEY_VERTICALRECOIL = "_verticalRecoil";

    public const string KEY_SHOTSPREAD = "_shotSpread";
    public const string KEY_BASESCATTER = "_baseScatter";
    public const string KEY_SCATTERINCRIMENT = "_scatterIncriment";
    public const string KEY_MAXSCATTER = "_maxScatter";

    public const string KEY_RELOADTIME = "_reloadTime";
    public const string KEY_ERGONOMICS = "_ergonomics";
    public const string KEY_MAXRANGE = "_maxRange";
    public override void Apply(ItemData asset, string[] fields)
    {
        base.Apply(asset, fields);

        // 2. Gun固有の型にキャストして、追加データを埋める
        if (!(asset is GunData gunData)) return;

        SetField(gunData, KEY_TARGETAMMO, fields);
        SetField(gunData, KEY_ISINTERNALMAGAZINE, fields);
        SetField(gunData, KEY_INTERNALCAPACITY, fields);
        SetField(gunData, KEY_FIRETYPE, fields);
        SetField(gunData, KEY_BURSTCOUNT, fields);
        SetField(gunData, KEY_SIMULNUM, fields);
        SetField(gunData, KEY_RPM, fields);
        SetField(gunData, KEY_VELOCITY, fields);
        SetField(gunData, KEY_HORIZONTALRECOIL, fields);
        SetField(gunData, KEY_VERTICALRECOIL, fields);
        SetField(gunData, KEY_SHOTSPREAD, fields);
        SetField(gunData, KEY_BASESCATTER, fields);
        SetField(gunData, KEY_SCATTERINCRIMENT, fields);
        SetField(gunData, KEY_MAXSCATTER, fields);
        SetField(gunData, KEY_RELOADTIME, fields);
        SetField(gunData, KEY_ERGONOMICS, fields);
        SetField(gunData, KEY_MAXRANGE, fields);
    }
}