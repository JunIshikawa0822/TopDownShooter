using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WeaponVisualLoader
{
    //これが実質的な「見た目用Factory」の役割
    // public async UniTask<GameObject> LoadVisualAsync(AssetReferenceGameObject reference)
    // {
    //     // 1. ログを出して、ここまでは到達しているか確認
    //     Debug.Log("--- LoadVisualAsync: Start (Dummy Mode) ---");

    //     if (reference == null) return null;

    //     // 2. 実際のロードは行わず、1フレームだけ待機
    //     await UniTask.Yield();

    //     // 3. 代わりのCubeを生成して返す
    //     GameObject dummy = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //     dummy.name = "Dummy_Weapon_Visual";
        
    //     Debug.Log("--- LoadVisualAsync: Success with Dummy Cube ---");
    //     return dummy;
    // }

    public async UniTask<GameObject> LoadVisualAsync(AssetReferenceGameObject reference)
    {
        if (reference == null) return null;

        // Handleを明示的に取得して待機する
        AsyncOperationHandle<GameObject> handle = reference.InstantiateAsync();
        await handle.ToUniTask();

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }
        else
        {
            Debug.LogError("Addressable Instantiate Failed");
            return null;
        }
    }  
}