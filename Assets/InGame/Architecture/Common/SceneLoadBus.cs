using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.Net.Security;

public class SceneLoadBus
{
    //ロード時のコールバック登録に関するリクエストが投げられたことを知らせるイベント
    public event Action<SceneLoadCallbackRequest> OnRequestRegisterCallback;
    public event Action<SceneLoadRequest> OnRequestLoadScene;
    public void RequestRegisterCallback(SceneType scene, Action<ISceneEntryPoint> callback)
    {
        OnRequestRegisterCallback?.Invoke(new SceneLoadCallbackRequest(scene, callback));
    }

    public void RequestLoadScene(SceneType scene, RequestLoadState loadState)
    {
        OnRequestLoadScene?.Invoke(new SceneLoadRequest(scene, loadState));
    } 
}
