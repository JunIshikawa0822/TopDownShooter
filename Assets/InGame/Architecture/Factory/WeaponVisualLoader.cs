using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class WeaponVisualLoader
{
    //これが実質的な「見た目用Factory」の役割
    public async UniTask<GameObject> LoadVisualAsync(AssetReferenceGameObject reference)
    {
        if (reference == null) return null;

        return await reference.InstantiateAsync();
    }
}