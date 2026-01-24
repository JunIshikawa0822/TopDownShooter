using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Collections.Generic;
using System.Reflection;
using System;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

#if UNITY_EDITOR
public abstract class DataImportBase
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
        string addressableName = fields[index];
        if (string.IsNullOrEmpty(addressableName)) return;

        ItemVisualData visual = DataObjectFactory.GetOrCreate(typeof(ItemVisualData), addressableName, "") as ItemVisualData;

        if (visual != null)
        {
            SetField(visual, "_prefab", fields);
            SetField(visual, "_width", fields);
            SetField(visual, "_height", fields);
            SetField(visual, "_icon", fields);
            SetField(visual, "_pickupSound", fields);
            SetField(visual, "_useSound", fields);

            EditorUtility.SetDirty(visual);

            //ItemData本体に紐づける
            SetFieldDirect(asset, "_visualData", visual);
        }
    }

    //シートの「列名」を指定して、セルから値を取ってセットする
    protected void SetField(object obj, string fieldName, string[] fields)
    {
        if (!_columnMap.TryGetValue(fieldName, out int index) || index >= fields.Length) return;
        
        //中身は「文字列」であることが確定している
        string val = fields[index];
        if (string.IsNullOrEmpty(val)) return;

        ApplyToField(obj, fieldName, val);
    }

    //すでに生成済みの「オブジェクト」を直接セットする
    protected void SetFieldDirect(object obj, string fieldName, object value)
    {
        if (value == null) return;
        ApplyToField(obj, fieldName, value);
    }

    private void ApplyToField(object obj, string fieldName, object value)
    {
        FieldInfo field = FindFieldIncludingBase(obj.GetType(), fieldName);
        if (field == null) return;

        // ここで一括変換
        object finalValue = ConvertValue(field.FieldType, value);
        field.SetValue(obj, finalValue);
    }

    protected virtual object ConvertValue(Type targetType, object value)
    {
        if (value == null) return null;
        string stringVal = value.ToString();

        //空文字や「None」の処理
        if (string.IsNullOrEmpty(stringVal) || stringVal.Equals("None", System.StringComparison.OrdinalIgnoreCase))
            return null;

        //基本型への変換
        if (targetType == typeof(string)) return stringVal;
        if (targetType == typeof(int)) return int.Parse(stringVal);
        if (targetType == typeof(float)) return float.Parse(stringVal);
        if (targetType == typeof(bool)) return bool.Parse(stringVal);
        if (targetType.IsEnum) return Enum.Parse(targetType, stringVal, true);
        if (targetType == typeof(Vector3))
        {
            string[] s = stringVal.Replace(" ", "").Split(',');
            if (s.Length < 3)
            {
                Debug.LogWarning($"x, y, zの3種が指定されていません");
                return Vector3.zero;
            }

            return new Vector3(float.Parse(s[0]), float.Parse(s[1]), float.Parse(s[2]));
        }

        //変換対象が値ではなくAssetReferenceだった場合
        //valueがAddressableNameになる
        if (typeof(AssetReference).IsAssignableFrom(targetType))
        {
            // 1. まず、オブジェクト実体（VisualDataなど）が直接渡された場合を先にチェック
            if (value is ScriptableObject so)
            {
                string path = AssetDatabase.GetAssetPath(so);
                string guid = AssetDatabase.AssetPathToGUID(path);
                if (!string.IsNullOrEmpty(guid))
                {
                    return Activator.CreateInstance(targetType, guid);
                }
            }

            // 2. 次に、文字列（Address名）として渡された場合をチェック
            string address = value.ToString();
            if (!string.IsNullOrEmpty(address))
            {
                AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
                AddressableAssetEntry entry = settings.FindAssetEntry(address);
                if (entry != null)
                {
                    return Activator.CreateInstance(targetType, entry.guid);
                }
                // 実体でもなく、Address名でもない場合のみ警告を出す
                Debug.LogWarning($"[Import] '{address}' を AssetReference に変換できません。Address登録がないか、実体が不正です。");
            }
            return null;
        }

        //そのまま代入可能なオブジェクト（VisualData等）の場合
        if (targetType.IsAssignableFrom(value.GetType())) return value;

        return null;
    }

    private FieldInfo FindFieldIncludingBase(Type type, string fieldName)
    {
        while (type != null)
        {
            //fieldを探すメソッドの有効範囲は「そのクラス」まで
            FieldInfo field = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
            if (field != null) return field;
            //親クラスにまで遡ることで全てのフィールドを調べる
            type = type.BaseType;
        }

        Debug.Log($"{fieldName}が見つかりません 誤記？");
        return null;
    }
}

#endif