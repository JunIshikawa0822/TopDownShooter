using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : ASystem
{
    private readonly Dictionary<string, ISceneEntryPoint> _loadedScenes = new();
    private readonly Dictionary<string, SceneLoadState> _sceneStates = new();
    private Dictionary<string, List<Action<ISceneEntryPoint>>> _onLoadCallbacks = new();
    
    public override void OnSetUp()
    {
        sceneLoadBus.OnRequestLoadCallback += RegisterLoadRequest;
    }

    //sceneLoadBus経由で他のクラスからシーンロードの際の条件登録を受ける
    private void RegisterLoadRequest(SceneLoadRequest request)
    {
        if (!_onLoadCallbacks.TryGetValue(request.TargetSceneName, out List<Action<ISceneEntryPoint>> list))
        {
            list = new List<Action<ISceneEntryPoint>>();
            _onLoadCallbacks[request.TargetSceneName] = list;
        }
        list.Add(request.Callback);
    }

    public IEnumerator LoadScene(string sceneName)
    {
        if (_sceneStates.TryGetValue(sceneName, out SceneLoadState state))
        {
            if (state == SceneLoadState.Loading || state == SceneLoadState.Loaded)
                yield break;
        }

        //アンロード中
        _sceneStates[sceneName] = SceneLoadState.Loading;

        //シーンをロード
        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        yield return op;

        Scene scene = SceneManager.GetSceneByName(sceneName);

        //シーンが有効でないのならステータスをNoneに
        if (!scene.IsValid())
        {
            _sceneStates[sceneName] = SceneLoadState.None;
            yield break;
        }

        ISceneEntryPoint entry = null;
        foreach (GameObject root in scene.GetRootGameObjects())
        {
            if (root.TryGetComponent<ISceneEntryPoint>(out entry))
            {
                break;
            }
        }

        if (entry == null)
        {
            Debug.LogWarning($"Scene {sceneName} に EntryPoint が見つかりません");
            yield break;
        }

        _loadedScenes[sceneName] = entry;
        _sceneStates[sceneName] = SceneLoadState.Loaded;

        //登録されているコールバックを実行
        if (_onLoadCallbacks.TryGetValue(sceneName, out List<Action<ISceneEntryPoint>> callbacks))
        {
            foreach (Action<ISceneEntryPoint> callback in callbacks)
            {
                callback?.Invoke(entry);
            }
        }
    }

    public IEnumerator UnloadScene(string sceneName)
    {
        //sceneNameに対応したロードステータスが登録されていないならそもそもアンロードしない
        if (!_sceneStates.TryGetValue(sceneName, out SceneLoadState state))
            yield break;

        //ロードステータスがロード済みでない場合ならそもそもアンロードしない
        if (state != SceneLoadState.Loaded)
            yield break;

        //アンロード中
        _sceneStates[sceneName] = SceneLoadState.Unloading;

        AsyncOperation op = SceneManager.UnloadSceneAsync(sceneName);
        yield return op;

        _loadedScenes.Remove(sceneName);
        _sceneStates[sceneName] = SceneLoadState.None;
    }

    public override void OnDispose()
    {
        sceneLoadBus.OnRequestLoadCallback -= RegisterLoadRequest;
    }
}
