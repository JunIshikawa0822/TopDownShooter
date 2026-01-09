using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class ASceneEntryPointBase : MonoBehaviour, ISceneEntryPoint
{
    private readonly Dictionary<Type, object> _dependenciesDic = new();

    protected virtual void Awake()
    {
        BuildDependencies();
        OnSetUp();
    }
    /// <summary>
    /// BuildDependencies内で、Registerメソッドを用いて外部に公開する型とインスタンスを登録する
    /// </summary>
    protected abstract void BuildDependencies();
    protected virtual void OnSetUp()
    {
        
    }

    //外部への公開をする型とインスタンスを登録する処理
    protected void RegisterDependency<T>(T instance) where T : class
    {
        if (instance == null)
            throw new InvalidOperationException($"{typeof(T).Name} is null");

        Type type = typeof(T);

        if (_dependenciesDic.ContainsKey(type))
            throw new InvalidOperationException($"{type.Name} already registered");

        _dependenciesDic.Add(type, instance);
    }

    //外部から依存を取得するために必要な処理を公開
    public bool TryGetDependency<T>(out T dependency) where T : class
    {
        if (_dependenciesDic.TryGetValue(typeof(T), out var obj))
        {
            dependency = obj as T;
            return dependency != null;
        }

        dependency = null;
        return false;
    }
}