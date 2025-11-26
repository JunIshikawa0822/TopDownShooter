using System;
using Game.Data;
using UnityEngine;

public class Factory_Handgun : IFactory<AWeaponBase>
{
    //private IObjectPool<Bullet> _bulletPool;
    private Handgun _handgunPrefab;
    public Factory_Handgun(Handgun handgunPrefab /*, IObjectPool<Bullet> bulletPool*/)
    {
        _handgunPrefab = handgunPrefab;
        //_bulletPool = bulletPool;
    }

    public AWeaponBase ObjectInstantiate()
    {
        Handgun newHandgun = GameObject.Instantiate(_handgunPrefab);
        //newHandgun.SetBulletPool(_bulletPool);

        return newHandgun;
    }
}
