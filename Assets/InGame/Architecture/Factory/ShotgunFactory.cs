using System;
using UnityEngine;

public class Factory_Shotgun : IFactory<AWeaponBase>
{
    //private IObjectPool<Bullet> _bulletPool;
    private Shotgun _shotgunPrefab;
    public Factory_Shotgun(Shotgun shotgunPrefab/*, IObjectPool<Bullet> bulletPool*/)
    {
        _shotgunPrefab = shotgunPrefab;
        //_bulletPool = bulletPool;
    }

    public AWeaponBase ObjectInstantiate()
    {
        Shotgun newShotgun = GameObject.Instantiate(_shotgunPrefab);
        //newShotgun.SetBulletPool(_bulletPool);

        return newShotgun;
    }
}
