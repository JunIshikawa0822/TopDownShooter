using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableProvider
{
    // 一括解放のためのハンドルリスト
    private readonly List<AsyncOperationHandle> _handles = new();

    // 個別解放を可能にするため「アセット本体」から「ハンドル」を逆引きする辞書
    private readonly Dictionary<object, AsyncOperationHandle> _assetToHandle = new();

    /// <summary>
    /// アセットをロードして内部でハンドルを追跡
    /// </summary>
    public async UniTask<T> LoadAssetAsync<T>(AssetReferenceT<T> reference) where T : Object
    {
        if (reference == null) return null;

        AsyncOperationHandle<T> handle = reference.LoadAssetAsync();
        _handles.Add(handle);

        await handle.ToUniTask();

        if (handle.Result != null && !_assetToHandle.ContainsKey(handle.Result))
        {
            _assetToHandle.Add(handle.Result, handle);
        }

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            return handle.Result;
        }
        else
        {
            Debug.LogError("Addressable LoadAsset Failed");
            return null;
        }
    }

    /// <summary>
    /// GameObjectを生成し、内部でハンドルを追跡
    /// </summary>
    public async UniTask<GameObject> InstantiateAsync(AssetReferenceGameObject reference, Transform parent = null)
    {
        if (reference == null) return null;

        AsyncOperationHandle<GameObject> handle = reference.InstantiateAsync(parent);
        _handles.Add(handle);
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

    /// <summary>
    /// 個別解放：ロードしたアセットを個別にリリース
    /// </summary>
    public void ReleaseAsset(object asset)
    {
        if (asset == null) return;

        if (_assetToHandle.TryGetValue(asset, out var handle))
        {
            if (handle.IsValid()) Addressables.Release(handle);
            _assetToHandle.Remove(asset);
            _handles.Remove(handle);
        }
        else
        {
            // 直接アセットをキーにリリースを試みる
            Addressables.Release(asset);
        }
    }

    /// <summary>
    /// 個別解放：生成したGameObjectを個別に破棄・リリース
    /// </summary>
    public void ReleaseInstance(GameObject instance)
    {
        if (instance == null) return;
        Addressables.ReleaseInstance(instance);
        //Instantiate系は内部でキャッシュが動いているため
        //リストからの削除はDisposeAll時にIsValidチェックで行う
    }

    /// <summary>
    /// 一括解放：このProviderで管理している全てのアセットとインスタンスを解放
    /// </summary>
    public void DisposeAll()
    {
        foreach (AsyncOperationHandle handle in _handles)
        {
            if (handle.IsValid())
            {
                Addressables.Release(handle);
            }
        }
        _handles.Clear();
        _assetToHandle.Clear();
    }
}
