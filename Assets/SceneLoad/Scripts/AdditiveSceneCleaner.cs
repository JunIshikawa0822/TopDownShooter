using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneCleaner : MonoBehaviour
{
    [Header("削除対象にするコンポーネント型（文字列で指定）")]
    [SerializeField] private List<string> targetTypes = new List<string> 
    { 
        "Camera", 
        "Light", 
        "EventSystem", 
        "AudioListener" 
    };

    [Header("設定")]
    [SerializeField] private bool destroyGameObject = true; // コンポーネントだけでなくGameObjectごと消すか

    public void Execute()
    {
        // 1. このスクリプトが配置されている「シーン」を取得
        Scene myScene = gameObject.scene;

        foreach (string typeName in targetTypes)
        {
            // 2. 文字列から型情報を取得
            Type type = GetTypeByName(typeName);
            if (type == null) continue;

            // 3. 全ての該当コンポーネントを検索
            UnityEngine.Object[] foundComponents = FindObjectsByType(type, FindObjectsSortMode.None);

            foreach (Component comp in foundComponents)
            {
                // 4. 「自分と同じシーン」かつ「重複（他に同じ型のものが別シーンにある）」か判定
                if (comp.gameObject.scene == myScene && IsDuplicate(comp, type))
                {
                    if (destroyGameObject)
                    {
                        //Debug.Log($"[Cleaner] 重複を検知して削除しました: {comp.gameObject.name} ({typeName})");
                        Destroy(comp.gameObject);
                    }
                    else
                    {
                        //Debug.Log($"[Cleaner] 重複を検知してSetActiveをオフにしました: {comp.gameObject.name} ({typeName})");
                        comp.gameObject.SetActive(false);
                    }  
                }
            }
        }
    }

    private bool IsDuplicate(Component comp, Type type)
    {
        UnityEngine.Object[] all = FindObjectsByType(type, FindObjectsSortMode.None);
        foreach (Component other in all)
        {
            // 自分とは別のシーンに、既に同じ型のコンポーネントが存在するか？
            if (other.gameObject.scene != comp.gameObject.scene)
            {
                // Directional Lightの場合は、タイプまでチェックするなどの個別ロジックを挟むとより安全
                if (other is Light l1 && comp is Light l2)
                {
                    if (l1.type != LightType.Directional || l2.type != LightType.Directional) continue;
                }
                
                return true; 
            }
        }
        return false;
    }

    private Type GetTypeByName(string name)
    {
        // UnityEngine名前空間などの主要な場所から型を検索
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type t = assembly.GetType(name) ?? assembly.GetType("UnityEngine." + name) ?? assembly.GetType("UnityEngine.EventSystems." + name);
            if (t != null) return t;
        }
        return null;
    }
}
