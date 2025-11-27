using System;
using UnityEngine;

public class Factory_AssultRifle_Ray : IFactory<AWeaponBase>
{
    // private IObjectPool<Bullet> _bulletPool;
    private AssultRifle_Ray _assultRiflePrefab;
    public Factory_AssultRifle_Ray(AssultRifle_Ray assultRiflePrefab/*, IObjectPool<Bullet> bulletPool*/)
    {
        _assultRiflePrefab = assultRiflePrefab;
        // _bulletPool = bulletPool;
    }

    public AWeaponBase ObjectInstantiate()
    {
        AssultRifle_Ray newAssultRifle = GameObject.Instantiate(_assultRiflePrefab);
        // newAssultRifle.SetBulletPool(_bulletPool);

        return newAssultRifle;
    }
}
