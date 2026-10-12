using UnityEngine;
using System;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using System.IO;

#if UNITY_EDITOR
public static class DataObjectFactory
{
    // 保存先のデフォルトルート（必要に応じて変更してください）
    private const string _saveRoot = "Assets/GameData/Items/Generates";

    public static ScriptableObject GetOrCreate(Type type, string addressableName, string savePath)
    {
        AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;
        AddressableAssetEntry entry = settings.FindAssetEntry(addressableName);
        
        ScriptableObject asset = null;
        //
        if (entry != null)
        {
            asset = AssetDatabase.LoadAssetAtPath(entry.AssetPath, type) as ScriptableObject;
        }
        else
        {
            //未指定の場合
            if(savePath == "")
            {
                savePath = _saveRoot;
            }

            //生成部分
            if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);
            string path = Path.Combine(savePath, $"{addressableName}.asset");

            //C#のオブジェクトとして使いたいだけならCreateInstance　メモリ上に作るだけ
            asset = ScriptableObject.CreateInstance(type);
            //アセットとして残す
            AssetDatabase.CreateAsset(asset, path);

            //デフォルトグループを取得（または特定のグループを指定）
            AddressableAssetGroup group = settings.DefaultGroup;
            AddressableAssetEntry newEntry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(path), group);

            // Address（名前）をシートの指定通りに書き換える
            newEntry.address = addressableName;

            //変更がたったことを設定
            settings.SetDirty(AddressableAssetSettings.ModificationEvent.EntryMoved, newEntry, true);
        }

        //AssetDatabase.SaveAssets();は非常に重いのでここではやらない
        return asset;
    }
}

#endif