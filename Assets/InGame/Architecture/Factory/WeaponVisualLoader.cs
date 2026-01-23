using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
public class WeaponVisualLoader
{
    //これが実質的な「見た目用Factory」の役割
    public async Task<GameObject> LoadVisualAsync(AssetReferenceGameObject reference)
    {
        if (reference == null) return null;

        GameObject weaponVisualInstance = await reference.InstantiateAsync().Task;
        return weaponVisualInstance;
    }
}