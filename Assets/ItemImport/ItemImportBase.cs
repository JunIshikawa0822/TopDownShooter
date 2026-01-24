using UnityEngine;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

public abstract class ItemImportBase
{
    protected Dictionary<string, int> _columnMap;

    // 窓口から辞書を受け取る
    public void SetMap(Dictionary<string, int> map)
    {
        _columnMap = map;
    }

    // 全てのデータの基本となる変換
    public virtual void Apply(ItemData asset, string[] fields)
    {
        SetField(asset, "_id", fields);
        SetField(asset, "_displayName", fields);
        SetField(asset, "_description", fields);
        SetField(asset, "_weight", fields);
        SetField(asset, "_price", fields);
        SetField(asset, "_stackableNumber", fields);
        SetField(asset, "_itemType", fields);
        SetField(asset, "_tags", fields);
        
        //ここからはVisualDataをセットする場所
        if (!_columnMap.TryGetValue("_visualData", out int index)) return;
        string path = fields[index];

        ItemVisualData visual = DataObjectFactory.GetOrCreate<ItemVisualData>(path);

        if (visual != null)
        {
            SetAddressableField(visual, "_prefab", fields);
            
            SetField(visual, "_width", fields);
            SetField(visual, "_height", fields);

            SetAddressableField(visual, "_icon", fields);
            SetAddressableField(visual, "_pickupSound", fields);
            SetAddressableField(visual, "_useSound", fields);

            EditorUtility.SetDirty(visual);

            //ItemData本体に紐づける
            SetFieldDirect(asset, "_visualData", visual);
        }
    }

    //列名から値を安全に取得し、リフレクションでセットする補助関数
    protected void SetField(object obj, string fieldName, string[] fields)
    {
        if (!_columnMap.TryGetValue(fieldName, out int index) || index >= fields.Length) return;
        string val = fields[index];
        
        if (string.IsNullOrEmpty(val)) return;

        FieldInfo field = FindFieldIncludingBase(obj.GetType(), fieldName);
        if (field == null) return;

        // 型変換してセット
        field.SetValue(obj, ConvertValue(field.FieldType, val));
    }

    //すでに型が確定しているインスタンスをリフレクションで無理やり変数に突っ込む
    protected void SetFieldDirect(object obj, string fieldName, object value)
    {
        FieldInfo field = FindFieldIncludingBase(obj.GetType(), fieldName);
        if (field != null) field.SetValue(obj, value);
    }

    protected void SetAddressableField(object obj, string fieldName, string[] fields)
    {
        if (!_columnMap.TryGetValue(fieldName, out int index)) return;
        string address = fields[index];

        //セルが完全に空、または「None」と書かれていたら、正常な「何もしない」として扱う
        if (string.IsNullOrEmpty(address) || address.Equals("None", StringComparison.OrdinalIgnoreCase)) return;

        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        AddressableAssetEntry entry = settings.FindAssetEntry(address);

        if (entry != null)
        {
            FieldInfo field = FindFieldIncludingBase(obj.GetType(), fieldName);
            if (field == null) return;

            // Activatorを使って適切なAssetReference型（GameObject用、Sprite用等）を生成
            object assetRef = Activator.CreateInstance(field.FieldType, entry.guid);
            field.SetValue(obj, assetRef);
        }
        else
        {
            Debug.LogError($"[Import] Addressable 「{address}」 がないよ??");
        }
    }

    private FieldInfo FindFieldIncludingBase(Type type, string fieldName)
    {
        while (type != null)
        {
            //fieldを探すが、有効範囲はそのクラスまで
            FieldInfo field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (field != null) return field;
            //親クラスにまで遡る必要あり
            type = type.BaseType;
        }
        return null;
    }

    protected virtual object ConvertValue(Type type, string val)
    {
        if (type == typeof(string)) return val;
        if (type == typeof(int)) return int.Parse(val);
        if (type == typeof(float)) return float.Parse(val);
        if (type == typeof(bool)) return bool.Parse(val);
        
        // Enum & Flags 対応
        if (type.IsEnum) return Enum.Parse(type, val, true);

        // Vector3 対応 (x, y, z)
        if (type == typeof(Vector3))
        {
            string[] s = val.Replace(" ", "").Split(',');
            return new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
        }

        return null;
    }
}