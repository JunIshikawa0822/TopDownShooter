using System;

public readonly struct SceneLoadRequest
{
    public readonly SceneType _targetScene;
    public readonly RequestLoadState _loadState;

    public SceneType TargetScene => _targetScene;
    public RequestLoadState State => _loadState;

    public SceneLoadRequest(SceneType scene, RequestLoadState state)
    {
        _targetScene = scene;
        _loadState = state;
    }
}
