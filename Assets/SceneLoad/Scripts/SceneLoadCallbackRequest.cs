using System;

public readonly struct SceneLoadCallbackRequest
{
    public readonly SceneType _targetScene;
    public readonly Action<ISceneEntryPoint> _callback;

    public SceneType TargetScene => _targetScene;
    public Action<ISceneEntryPoint> Callback => _callback;

    public SceneLoadCallbackRequest(SceneType scene, Action<ISceneEntryPoint> callback)
    {
        _targetScene = scene;
        _callback = callback;
    }
}
