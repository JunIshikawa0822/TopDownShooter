using System;

public struct SceneLoadRequest
{
    public string TargetSceneName;
    public Action<ISceneEntryPoint> Callback;
}
