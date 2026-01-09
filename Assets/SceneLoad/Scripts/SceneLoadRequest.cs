using System;

public struct SceneLoadRequest
{
    public SceneType TargetSceneName;
    public Action<ISceneEntryPoint> Callback;
}
