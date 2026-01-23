using System;
using UnityEngine;

public class Factory_SniperRifle : IFactory<AWeaponBase>
{
    private SniperRifle _gunPrefab;
    private GunService _gunService;
    private BulletService _bulletService;
    public Factory_SniperRifle(SniperRifle gunPrefab, GunService gunService, BulletService bulletService)
    {
        _gunPrefab = gunPrefab;
        _gunService = gunService;
        _bulletService = bulletService;
    }

    public AWeaponBase ObjectInstantiate()
    {
        SniperRifle gun = GameObject.Instantiate(_gunPrefab);

        _gunPrefab.SetGunSurvice(_gunService);
        _gunPrefab.SetBulletSurvice(_bulletService);
        _gunService.RegisterGun(gun);

        return gun;
    }
}
