using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WeaponVisualLoader
{
    //これが実質的な「見た目用Factory」の役割
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