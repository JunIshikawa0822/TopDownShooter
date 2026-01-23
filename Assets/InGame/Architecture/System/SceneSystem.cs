using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.XR; //SceneInstanceに必要

public class SceneSystem : ASystem, IOnUpdate
{
    public bool IsActiveForUpdate => true;
    private Dictionary<SceneType, List<Action<ISceneEntryPoint>>> _onLoadCallbacks = new();
    private readonly Queue<SceneLoadRequest> _sceneLoadRequestQueue = new();
    private readonly Dictionary<SceneType, SceneLoadState> _sceneStates = new();

    // 重要: AddressablesでのアンロードにはSceneInstanceの保持が必須
    private readonly Dictionary<SceneType, SceneInstance> _loadedSceneInstances = new();
    private bool _isProcessing = false;

    private SceneLoadRequest? _currentRequest;
    public override void OnSetUp()
    {
        sceneLoadBus.OnRequestRegisterCallback += RegisterCallbackRequest;
        sceneLoadBus.OnRequestLoadScene += RegisterSceneLoadRequest;
    }

    public void OnUpdate()
    {
        if(_isProcessing || _sceneLoadRequestQueue.Count == 0) return;
        _ = ProcessNextRequestAsync();
    }

    // private void ProcessSceneLoadRequest()
    // {
    //     if(_currentRequest != null) return;
    //     if(_sceneLoadRequestQueue.Count == 0)return;

    //     SceneLoadRequest request = _sceneLoadRequestQueue.Dequeue();
        
    //     switch(request.State)
    //     {
    //         case RequestLoadState.Load : LoadScene(request);
    //         break;
    //         case RequestLoadState.Unload : UnloadScene(request);
    //         break;

    //         default:
    //         break;
    //     }
    // }

    private async Task ProcessNextRequestAsync()
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

    private async Task LoadSceneAsync(SceneLoadRequest request)
    {
        if (_sceneStates.TryGetValue(request.TargetScene, out SceneLoadState state))
        {
            if (state == SceneLoadState.Loading || state == SceneLoadState.Loaded) return;
        }

        _sceneStates[request.TargetScene] = SceneLoadState.Loading;

        // Addressablesでのロード実行
        AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(request.TargetScene.ToString(), LoadSceneMode.Additive);
        SceneInstance instance = await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
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
        else
        {
            _sceneStates[request.TargetScene] = SceneLoadState.None;
            Debug.LogError($"Scene {request.TargetScene} のロードに失敗しました");
        }
    }

    private async Task UnloadSceneAsync(SceneLoadRequest request)
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
        await Addressables.UnloadSceneAsync(instance).Task;

        _loadedSceneInstances.Remove(request.TargetScene);
        _sceneStates[request.TargetScene] = SceneLoadState.Unloaded;
    }

    //シーンのロード
    // private void LoadScene(SceneLoadRequest request)
    // {
    //     //当該シーンがLoadingないしはLoad済みであればロードしない
    //     if (_sceneStates.TryGetValue(request.TargetScene, out SceneLoadState state))
    //     {
    //         if (state == SceneLoadState.Loading || state == SceneLoadState.Loaded) return;
    //     }

    //     //処理中のリクエストとして設定
    //     _currentRequest = request;
    //     //当該シーンのロード状況を「ロード中」に設定
    //     _sceneStates[request.TargetScene] = SceneLoadState.Loading;

    //     //ロード開始
    //     AsyncOperation op = SceneManager.LoadSceneAsync(request.TargetScene.ToString(), LoadSceneMode.Additive);
    //     op.completed += OnLoadCompleted;
    // }

    // private void UnloadScene(SceneLoadRequest request)
    // {
    //     if (_sceneStates.TryGetValue(request.TargetScene, out SceneLoadState state))
    //     {
    //         //当該シーンがUnloadingないしはUnload済みであればアンロードしない
    //         if (state == SceneLoadState.Unloading || state == SceneLoadState.Unloaded) return;
    //     }
    //     //当該シーンに対応するシーンが登録されていないならアンロードしない
    //     else return;

    //     //処理中のリクエストとして設定
    //     _currentRequest = request;
    //     //当該シーンのロード状況を「ロード中」に設定
    //     _sceneStates[request.TargetScene] = SceneLoadState.Unloading;
    //     //アンロード開始
    //     AsyncOperation op = SceneManager.UnloadSceneAsync(request.TargetScene.ToString());
    //     op.completed += OnUnloadCompleted;
    // }

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

    //ロードが完了したら発火される
    // private void OnLoadCompleted(AsyncOperation op)
    // {
    //     //何らかの理由でcurrentRequestがnullもしくは変更されているかを検知
    //     if(_currentRequest is not SceneLoadRequest req)
    //     {
    //         Debug.LogError("読み込んでいるリクエストが正しくありません");
    //         return;
    //     }

    //     //ロードされたシーンを探す
    //     Scene loadScene = SceneManager.GetSceneByName(req.TargetScene.ToString());

    //     if (!loadScene.IsValid())
    //     {
    //         _sceneStates[req.TargetScene] = SceneLoadState.None; return;
    //     }

    //     //エントリーポイントの読み込み
    //     ISceneEntryPoint entry = null;
    //     foreach (GameObject root in loadScene.GetRootGameObjects())
    //     {
    //         if (root.TryGetComponent<ISceneEntryPoint>(out entry)) break;
    //     }

    //     if (entry == null)
    //     {
    //         Debug.LogWarning($"Scene {req.TargetScene} に EntryPoint が見つかりません");
    //     }

    //     //_loadedScenesEntry[req.TargetScene] = entry;

    //     //登録されているコールバックを実行
    //     if (_onLoadCallbacks.TryGetValue(req.TargetScene, out List<Action<ISceneEntryPoint>> callbacks))
    //     {
    //         foreach (Action<ISceneEntryPoint> callback in callbacks)
    //         {
    //             callback?.Invoke(entry);
    //         }
    //     }

    //     //全て終わったら完了
    //     _sceneStates[req.TargetScene] = SceneLoadState.Loaded;
    //     _currentRequest = null;
    // }

    // private void OnUnloadCompleted(AsyncOperation op)
    // {
    //     //何らかの理由でcurrentRequestがnullもしくは変更されているかを検知
    //     if(_currentRequest is not SceneLoadRequest req)
    //     {
    //         Debug.LogError("読み込んでいるリクエストが正しくありません");
    //         return;
    //     }

    //     _sceneStates[req.TargetScene] = SceneLoadState.Unloaded;
    //     _currentRequest = null;
    // }

    public override void OnDispose()
    {
        sceneLoadBus.OnRequestRegisterCallback -= RegisterCallbackRequest;
    }
}
