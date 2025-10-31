using System;
using UnityEngine;

public class Gun_Rifle_Factory : IFactory<Rifle>
{
    private Rifle _rifle;
    private ObjectPool<Bullet> _objectPool;
    public Gun_Rifle_Factory(ObjectPool<Bullet> objectPool, Rifle rifle)
    {
        _objectPool = objectPool;
        _rifle = rifle;
    }

    public Rifle ObjectInstantiate()
    {
        Rifle newRifle = GameObject.Instantiate(_rifle);

        return newRifle;
    }
}
