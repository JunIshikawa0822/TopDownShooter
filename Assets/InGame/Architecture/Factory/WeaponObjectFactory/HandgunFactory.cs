using System;
using Game.Data;
using UnityEngine;

public class Factory_Handgun : IObjectFactory<AWeaponBase>
{
    //private IObjectPool<Bullet> _bulletPool
    private Handgun _gunPrefab;
    private GunService _gunService;
    private BulletService _bulletService;
    public Factory_Handgun(Handgun gunPrefab, GunService gunService, BulletService bulletService)
    {
        _gunPrefab = gunPrefab;
        _gunService = gunService;
        _bulletService = bulletService;
    }

    public AWeaponBase ObjectInstantiate()
    {
        Handgun gun = GameObject.Instantiate(_gunPrefab);

        gun.SetGunSurvice(_gunService);
        gun.SetBulletSurvice(_bulletService);
        _gunService.RegisterGun(gun);

        return gun;
    }
}
