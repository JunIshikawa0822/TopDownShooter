using UnityEditor;
using UnityEngine;
using System.IO;

public static class DataObjectFactory
{
    public static T GetOrCreate<T>(string assetPath) where T : ScriptableObject
    {
        if (string.IsNullOrEmpty(assetPath)) return null;

        //指定パスにアセットがあるか確認
        T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);

        // 2. なければ製作
        if (asset == null)
        {
            // フォルダがなければ作成
            string directory = Path.GetDirectoryName(assetPath);
            if (!Directory.Exists(directory)) Directory.CreateDirectory(directory);

            asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, assetPath);
            Debug.Log($"{typeof(T)}のScriptableObjectが{assetPath}に作成されました");
        }

        return asset;
    }
}