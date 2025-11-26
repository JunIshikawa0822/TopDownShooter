using Game.Data;
using UnityEngine;
using System.Collections.Generic;

public class GunService : IOnUpdate, IGunService
{
    public bool IsActiveForUpdate => true;
    private BulletService _bulletService;
    private Dictionary<IGun<GunRuntimeData>, GunState> _gunStates = new();

    public GunService(BulletService bulletService)
    {
        _bulletService = bulletService;
    }

    public void RegisterGun(IGun<GunRuntimeData> gun)
    {
        GunState state = new(){Gun = gun};
        _gunStates.Add(gun, state);
    }

    public void UnRegisterGun(IGun<GunRuntimeData> gun)
    {
        _gunStates.Remove(gun);
    }

    public bool CanShoot(IGun<GunRuntimeData> gun)
    {
        float last = _gunStates.TryGetValue(gun, out GunState t) ? t.LastShotTime : -999f;
        bool canShoot = Time.time - last >= 1f / gun.RuntimeData.FireRate;

        return canShoot;
    }

    public void RecordShotTime(IGun<GunRuntimeData> gun)
    {
        _gunStates[gun].LastShotTime = Time.time;
    }

    public Bullet GetBullet()
    {
        return _bulletService.GetBullet();
    }

    //銃がGunServiceに問い合わせるのは「intervalかどうか」だけ
    //今の所Updateは使わない
    public void OnUpdate()
    {
        
    }

    private class GunState
    {
        public IGun<GunRuntimeData> Gun;
        public float LastShotTime;
    }
}
