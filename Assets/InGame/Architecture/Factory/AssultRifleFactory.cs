using System;
using UnityEngine;

public class Factory_AssultRifle : IFactory<AWeaponBase>
{
    // private IObjectPool<Bullet> _bulletPool;
    private AssultRifle _assultRiflePrefab;
    public Factory_AssultRifle(AssultRifle assultRiflePrefab/*, IObjectPool<Bullet> bulletPool*/)
    {
        _assultRiflePrefab = assultRiflePrefab;
        // _bulletPool = bulletPool;
    }

    public AWeaponBase ObjectInstantiate()
    {
        AssultRifle newAssultRifle = GameObject.Instantiate(_assultRiflePrefab);
        // newAssultRifle.SetBulletPool(_bulletPool);

        return newAssultRifle;
    }
}
