using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public class SceneSystem : ASystem, IOnUpdate
{
    public bool IsActiveForUpdate => true;
    private Dictionary<SceneType, List<Action<ISceneEntryPoint>>> _onLoadCallbacks = new();
    private readonly Queue<SceneLoadRequest> _sceneLoadRequestQueue = new();
    private readonly Dictionary<SceneType, SceneLoadState> _sceneStates = new();

    // 重要: AddressablesでのアンロードにはSceneInstanceの保持が必須
    private readonly Dictionary<SceneType, SceneInstance> _loadedSceneInstances = new();
    private bool _isProcessing = false;

    public override void OnSetUp()
    {
        sceneLoadBus.OnRequestRegisterCallback += RegisterCallbackRequest;
        sceneLoadBus.OnRequestLoadScene += RegisterSceneLoadRequest;
    }

    public void OnUpdate()
    {
        if(_isProcessing || _sceneLoadRequestQueue.Count == 0) return;
        ProcessNextRequestAsync().Forget();
    }

    private async UniTaskVoid ProcessNextRequestAsync()
    {
        _isProcessing = true;
        SceneLoadRequest request = _sceneLoadRequestQueue.Dequeue();

        try
        {
            switch (request.State)
            {
                case RequestLoadState.Load:
                    await LoadSceneAsync(request);
                    break;
                case RequestLoadState.Unload:
                    await UnloadSceneAsync(request);
                    break;
            }
        }
        finally
        {
            _isProcessing = false;
        }
    }

    //sceneLoadBus経由で他のクラスから「シーンロード時に実行したいコールバック登録」を受ける
    private void RegisterCallbackRequest(SceneLoadCallbackRequest request)
    {
        if (!_onLoadCallbacks.TryGetValue(request.TargetScene, out List<Action<ISceneEntryPoint>> list))
        {
            list = new List<Action<ISceneEntryPoint>>();
            _onLoadCallbacks[request.TargetScene] = list;
        }
        list.Add(request.Callback);
    }

    //sceneLoadBus経由で他のクラスから「シーンロード/アンロードのリクエスト」を受ける 
    private void RegisterSceneLoadRequest(SceneLoadRequest request)
    {
        _sceneLoadRequestQueue.Enqueue(request);
    }

    private async UniTask LoadSceneAsync(SceneLoadRequest request)
    {
        if (_sceneStates.TryGetValue(request.TargetScene, out SceneLoadState state))
        {
            if (state == SceneLoadState.Loading || state == SceneLoadState.Loaded) return;
        }

        _sceneStates[request.TargetScene] = SceneLoadState.Loading;

        // Addressablesでのロード実行
        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(request.TargetScene.ToString(), LoadSceneMode.Additive);

        try
        {
            SceneInstance instance = await handle;

            _loadedSceneInstances[request.TargetScene] = instance;
            
            // エントリーポイントの取得とコールバック実行
            ISceneEntryPoint entry = GetEntryPoint(instance.Scene);

            if(entry == null)
            {
                Debug.LogWarning($"Scene {instance.Scene} に EntryPoint が見つかりません");
            }
            ExecuteCallbacks(request.TargetScene, entry);

            _sceneStates[request.TargetScene] = SceneLoadState.Loaded;
        }
        catch(OperationCanceledException)
        {
            
        }
        catch (Exception ex)
        {
            _sceneStates[request.TargetScene] = SceneLoadState.None;
            Debug.LogError($"Scene {request.TargetScene} ロード失敗: {ex.Message}");
        }
    }

    private async UniTask UnloadSceneAsync(SceneLoadRequest request)
    {
        if (!_loadedSceneInstances.TryGetValue(request.TargetScene, out SceneInstance instance))
        {
            return;
        }

        if (_sceneStates.TryGetValue(request.TargetScene, out SceneLoadState state))
        {
            if (state == SceneLoadState.Unloading || state == SceneLoadState.Unloaded) return;
        }

        _sceneStates[request.TargetScene] = SceneLoadState.Unloading;

        //Addressablesでのアンロード実行（SceneInstanceを渡す）
        await Addressables.UnloadSceneAsync(instance);

        _loadedSceneInstances.Remove(request.TargetScene);
        _sceneStates[request.TargetScene] = SceneLoadState.Unloaded;
    }

    private ISceneEntryPoint GetEntryPoint(Scene scene)
    {
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            if (root.TryGetComponent(out ISceneEntryPoint entry)) return entry;
        }
        return null;
    }

    private void ExecuteCallbacks(SceneType targetScene, ISceneEntryPoint entry)
    {
        if (_onLoadCallbacks.TryGetValue(targetScene, out List<Action<ISceneEntryPoint>> callbacks))
        {
            foreach (Action<ISceneEntryPoint> callback in callbacks)
            {
                callback?.Invoke(entry);
            }
        }
    }

    public override void OnDispose()
    {
        sceneLoadBus.OnRequestRegisterCallback -= RegisterCallbackRequest;

        //TODO: SceneSystemがOnDisposeされるときに、実行中のロードを止めたい場合は追加で書く
    }
}
