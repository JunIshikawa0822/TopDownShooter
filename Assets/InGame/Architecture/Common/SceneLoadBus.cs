using System.Collections.Generic;
using System;

public class SceneLoadBus
{
    // リクエストが投げられたことを知らせるイベント
    public event Action<SceneLoadRequest> OnRequestLoadCallback;
    public void RequestLoadCallback(string sceneName, Action<ISceneEntryPoint> callback)
    {
        OnRequestLoadCallback?.Invoke(new SceneLoadRequest
        { 
            TargetSceneName = sceneName,
            Callback = callback 
        });
    }
}
