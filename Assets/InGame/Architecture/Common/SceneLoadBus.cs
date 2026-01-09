using System.Collections.Generic;
using System;

public class SceneLoadBus
{
    // リクエストが投げられたことを知らせるイベント
    public event Action<SceneLoadRequest> OnRequestRegisterCallback;
    public void RequestRegisterCallback(SceneType sceneName, Action<ISceneEntryPoint> callback)
    {
        OnRequestRegisterCallback?.Invoke(new SceneLoadRequest
        { 
            TargetSceneName = sceneName,
            Callback = callback 
        });
    }
}
