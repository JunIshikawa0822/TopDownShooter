using Game.Data;
using System;
public class WeaponDataImport : DataImportBase
{
    public const string KEY_WEAPONWEAPONTYPE = "_weaponType";
    public const string KEY_WEAPONWEAPONANIMATION = "_weaponAnimation";
    public const string KEY_WEAPONEQUIPPABLETYPE = "_equippableTypes";
    public override void Apply(ItemData asset, string[] fields)
    {
        base.Apply(asset, fields);

        // 2. Gun固有の型にキャストして、追加データを埋める
        if (!(asset is WeaponData weapon)) return;

        SetField(weapon, KEY_WEAPONWEAPONTYPE, fields);
        SetField(weapon, KEY_WEAPONWEAPONANIMATION, fields);
        SetField(weapon, KEY_WEAPONEQUIPPABLETYPE, fields);

        //TODO: 付帯可能エンチャントの変換とScriptableObjectへのセット
        //SetField(weapon, KEY_WEAPONATTACHABLEENCHANTS, fields);
    }

    protected override object ConvertValue(Type targetType, object value)
    {
        string stringVal = value?.ToString();

        // ターゲットが配列 かつ 要素が Enum の場合の特殊処理
        if (targetType.IsArray && targetType.GetElementType().IsEnum && !string.IsNullOrEmpty(stringVal))
        {
            string[] items = stringVal.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            Type elementType = targetType.GetElementType();
            Array array = Array.CreateInstance(elementType, items.Length);

            for (int i = 0; i < items.Length; i++)
            {
                //空白や余計な改行を削除
                string trimmed = items[i].Trim();
                try
                {
                    array.SetValue(Enum.Parse(elementType, trimmed, true), i);
                }
                catch (Exception)
                {
                    UnityEngine.Debug.LogWarning($"[Import] Enum '{elementType.Name}' に値 '{trimmed}' が見つかりません。無視します。");
                }
            }
            return array;
        }

        return base.ConvertValue(targetType, value);
    }

}