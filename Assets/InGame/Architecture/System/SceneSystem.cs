using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : ASystem, IOnUpdate
{
    public bool IsActiveForUpdate => true;
    //private readonly Dictionary<SceneType, ISceneEntryPoint> _loadedScenesEntry = new();
    private Dictionary<SceneType, List<Action<ISceneEntryPoint>>> _onLoadCallbacks = new();
    private readonly Queue<SceneLoadRequest> _sceneLoadRequestQueue = new();
    private readonly Dictionary<SceneType, SceneLoadState> _sceneStates = new();
    private SceneLoadRequest? _currentRequest;
    public override void OnSetUp()
    {
        sceneLoadBus.OnRequestRegisterCallback += RegisterCallbackRequest;
        sceneLoadBus.OnRequestLoadScene += RegisterSceneLoadRequest;
    }

    public void OnUpdate()
    {
        ProcessSceneLoadRequest();
    }

    private void ProcessSceneLoadRequest()
    {
        if(_currentRequest != null) return;
        if(_sceneLoadRequestQueue.Count == 0)return;

        SceneLoadRequest request = _sceneLoadRequestQueue.Dequeue();
        
        switch(request.State)
        {
            case RequestLoadState.Load : LoadScene(request);
            break;
            case RequestLoadState.Unload : UnloadScene(request);
            break;

            default:
            break;
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

    //シーンのロード
    private void LoadScene(SceneLoadRequest request)
    {
        //当該シーンがLoadingないしはLoad済みであればロードしない
        if (_sceneStates.TryGetValue(request.TargetScene, out SceneLoadState state))
        {
            if (state == SceneLoadState.Loading || state == SceneLoadState.Loaded) return;
        }

        //処理中のリクエストとして設定
        _currentRequest = request;
        //当該シーンのロード状況を「ロード中」に設定
        _sceneStates[request.TargetScene] = SceneLoadState.Loading;

        //ロード開始
        AsyncOperation op = SceneManager.LoadSceneAsync(request.TargetScene.ToString(), LoadSceneMode.Additive);
        op.completed += OnLoadCompleted;
    }

    private void UnloadScene(SceneLoadRequest request)
    {
        if (_sceneStates.TryGetValue(request.TargetScene, out SceneLoadState state))
        {
            //当該シーンがUnloadingないしはUnload済みであればアンロードしない
            if (state == SceneLoadState.Unloading || state == SceneLoadState.Unloaded) return;
        }
        //当該シーンに対応するシーンが登録されていないならアンロードしない
        else return;

        //処理中のリクエストとして設定
        _currentRequest = request;
        //当該シーンのロード状況を「ロード中」に設定
        _sceneStates[request.TargetScene] = SceneLoadState.Unloading;
        //アンロード開始
        AsyncOperation op = SceneManager.UnloadSceneAsync(request.TargetScene.ToString());
        op.completed += OnUnloadCompleted;
    }

    //ロードが完了したら発火される
    private void OnLoadCompleted(AsyncOperation op)
    {
        //何らかの理由でcurrentRequestがnullもしくは変更されているかを検知
        if(_currentRequest is not SceneLoadRequest req)
        {
            Debug.LogError("読み込んでいるリクエストが正しくありません");
            return;
        }

        //ロードされたシーンを探す
        Scene loadScene = SceneManager.GetSceneByName(req.TargetScene.ToString());

        if (!loadScene.IsValid())
        {
            _sceneStates[req.TargetScene] = SceneLoadState.None; return;
        }

        //エントリーポイントの読み込み
        ISceneEntryPoint entry = null;
        foreach (GameObject root in loadScene.GetRootGameObjects())
        {
            if (root.TryGetComponent<ISceneEntryPoint>(out entry)) break;
        }

        if (entry == null)
        {
            Debug.LogWarning($"Scene {req.TargetScene} に EntryPoint が見つかりません");
        }

        //_loadedScenesEntry[req.TargetScene] = entry;

        //登録されているコールバックを実行
        if (_onLoadCallbacks.TryGetValue(req.TargetScene, out List<Action<ISceneEntryPoint>> callbacks))
        {
            foreach (Action<ISceneEntryPoint> callback in callbacks)
            {
                callback?.Invoke(entry);
            }
        }

        //全て終わったら完了
        _sceneStates[req.TargetScene] = SceneLoadState.Loaded;
        _currentRequest = null;
    }

    public IEnumerator UnloadScene(SceneType sceneName)
    {
        //sceneNameに対応したロードステータスが登録されていないならそもそもアンロードしない
        if (!_sceneStates.TryGetValue(sceneName, out SceneLoadState state))
            yield break;

        //ロードステータスがロード済みでない場合ならそもそもアンロードしない
        if (state != SceneLoadState.Loaded)
            yield break;

        //アンロード中
        _sceneStates[sceneName] = SceneLoadState.Unloading;

        AsyncOperation op = SceneManager.UnloadSceneAsync(sceneName.ToString());
        yield return op;

        //_loadedScenesEntry.Remove(sceneName);
        _sceneStates[sceneName] = SceneLoadState.None;
    }

    private void OnUnloadCompleted(AsyncOperation op)
    {
        //何らかの理由でcurrentRequestがnullもしくは変更されているかを検知
        if(_currentRequest is not SceneLoadRequest req)
        {
            Debug.LogError("読み込んでいるリクエストが正しくありません");
            return;
        }

        _sceneStates[req.TargetScene] = SceneLoadState.Unloaded;
        _currentRequest = null;
    }

    public override void OnDispose()
    {
        sceneLoadBus.OnRequestRegisterCallback -= RegisterCallbackRequest;
    }
}
