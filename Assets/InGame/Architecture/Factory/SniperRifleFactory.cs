using System;
using UnityEngine;

public class Factory_SniperRifle : IFactory<AWeaponBase>
{
    private IObjectPool<Bullet> _bulletPool;
    private SniperRifle _sniperRiflePrefab;
    public Factory_SniperRifle(SniperRifle sniperRiflePrefab, IObjectPool<Bullet> bulletPool)
    {
        _sniperRiflePrefab = sniperRiflePrefab;
        _bulletPool = bulletPool;
    }

    public AWeaponBase ObjectInstantiate()
    {
        SniperRifle newSniperRifle = GameObject.Instantiate(_sniperRiflePrefab);
        newSniperRifle.SetBulletPool(_bulletPool);

        return newSniperRifle;
    }
}
