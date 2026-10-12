using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class ObjectPool<T> : IObjectPool<T> where T : APooledObject
{
    private Stack<T> _pool;
    private GameObject _parent;
    private Transform _transform;
    private IObjectFactory<T> _factory;

    public ObjectPool(Transform poolTransform, IObjectFactory<T> factory, string name = "")
    {
        _pool = new Stack<T>();
        _parent = new GameObject(name);
        _transform = poolTransform;

        _factory = factory;

        _parent.transform.SetParent(_transform);
    }

    public void PoolSetUp(uint initPoolSize)
    {
        //Stackの初期化
        if (_factory == null)
        {
            return;
        }

        //とりあえずPoolSize分instanceを生成して、見えなくしておく
        for (int i = 0; i < initPoolSize; i++)
        {
            T instance = ObjectInstantiate();
            // Debug.Log(instance);
            if (instance == null) return;

            instance.gameObject.transform.SetParent(_parent.transform);
            instance.gameObject.SetActive(false);
            _pool.Push(instance);
        }
    }

    public T GetFromPool()
    {
        if (_factory == null)
        {
            return null;
        }

        // プールに在庫があればそれを使用、なければ新規作成
        if (_pool.Count < 1)
        {
            T newInstance = ObjectInstantiate();
            if (newInstance == null) return null;

            newInstance.gameObject.transform.SetParent(_parent.transform);
            return newInstance;
        }

        T nextInstance = _pool.Pop();
        nextInstance.gameObject.SetActive(true);
        return nextInstance;
    }

    private T ObjectInstantiate()
    {
        T instance = _factory.ObjectInstantiate();
        if (instance == null) return null;

        instance.SetPoolAction<T>(ReturnToPool);
        return instance;
    }

    // 弾をプールに戻す
    public void ReturnToPool(T pooledObject)
    {
        Stack<T> objectPool = _pool;

        objectPool.Push(pooledObject);
        pooledObject.transform.SetParent(_parent.transform);
        pooledObject.gameObject.SetActive(false);
    }
}
