using System;
using UnityEngine;

public class Factory_AssultRifle : IFactory<AWeaponBase>
{
    private AssultRifle _gunPrefab;
    private GunService _gunService;
    private BulletService _bulletService;
    public Factory_AssultRifle(AssultRifle gunPrefab, GunService gunService, BulletService bulletService)
    {
        _gunPrefab = gunPrefab;
        _gunService = gunService;
        _bulletService = bulletService;
    }

    public AWeaponBase ObjectInstantiate()
    {
        AssultRifle gun = GameObject.Instantiate(_gunPrefab);

        _gunPrefab.SetGunSurvice(_gunService);
        _gunPrefab.SetBulletSurvice(_bulletService);
        _gunService.RegisterGun(gun);

        return gun;
    }
}
