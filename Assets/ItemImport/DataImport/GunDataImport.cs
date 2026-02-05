using Game.Data;
using System;
public class GunDataImport : WeaponDataImport
{
    public const string KEY_GUNTARGETAMMO = "_targetAmmo";
    public const string KEY_GUNISINTERNALMAGAZINE = "_isInternalMagazine";
    public const string KEY_GUNFIRETYPE = "_fireType";
    public const string KEY_GUNBURSTCOUNT = "_burstCount";
    public const string KEY_GUNRPM = "_rpm";
    public const string KEY_GUNVELOCITY = "_velocity";
    public const string KEY_GUNHORIZONTALRECOIL = "_horizontalRecoil";
    public const string KEY_GUNVERTICALRECOIL = "_verticalRecoil";
    public const string KEY_GUNBASESPREAD = "_baseSpread";
    public const string KEY_GUNSPREADINCRIMENT = "_spreadIncriment";
    public const string KEY_GUNMAXSPREAD = "_maxSpread";
    public const string KEY_GUNRELOADTIME = "_reloadTime";
    public const string KEY_GUNERGONOMICS = "_ergonomics";
    public const string KEY_GUNMAXRANGE = "_maxRange";
    public override void Apply(ItemData asset, string[] fields)
    {
        base.Apply(asset, fields);

        // 2. Gun固有の型にキャストして、追加データを埋める
        if (!(asset is GunData gunData)) return;

        SetField(gunData, KEY_GUNTARGETAMMO, fields);
        SetField(gunData, KEY_GUNISINTERNALMAGAZINE, fields);
        SetField(gunData, KEY_GUNFIRETYPE, fields);
        SetField(gunData, KEY_GUNBURSTCOUNT, fields);
        SetField(gunData, KEY_GUNRPM, fields);
        SetField(gunData, KEY_GUNVELOCITY, fields);
        SetField(gunData, KEY_GUNHORIZONTALRECOIL, fields);
        SetField(gunData, KEY_GUNVERTICALRECOIL, fields);
        SetField(gunData, KEY_GUNBASESPREAD, fields);
        SetField(gunData, KEY_GUNSPREADINCRIMENT, fields);
        SetField(gunData, KEY_GUNMAXSPREAD, fields);
        SetField(gunData, KEY_GUNRELOADTIME, fields);
        SetField(gunData, KEY_GUNERGONOMICS, fields);
        SetField(gunData, KEY_GUNMAXRANGE, fields);
    }
}