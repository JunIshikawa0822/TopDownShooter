using Game.Data;
using UnityEngine;
using System.Collections.Generic;

public class GunService : IOnUpdate, IGunService
{
    public bool IsActiveForUpdate => true;
    private bool _isBulletConsume = true;
    private Dictionary<IGun<GunRuntime>, GunState> _gunStates = new();

    public bool IsBulletConsume => _isBulletConsume;

    public void RegisterGun(IGun<GunRuntime> gun)
    {
        GunState state = new() { Gun = gun, IsShooting = false };
        _gunStates.Add(gun, state);
    }

    public void UnRegisterGun(IGun<GunRuntime> gun)
    {
        _gunStates.Remove(gun);
    }

    public bool CanShoot(IGun<GunRuntime> gun)
    {
        float last = _gunStates.TryGetValue(gun, out GunState t) ? t.LastShotTime : -999f;
        bool canShoot = Time.time - last >= gun.GunRuntime.FireInterval;

        return canShoot;
    }

    public void RecordShotTime(IGun<GunRuntime> gun)
    {
        if (_gunStates.TryGetValue(gun, out GunState t))
        {
            t.LastShotTime = Time.time;
        }
        else
        {
            Debug.Log($"{gun}が見つかりません");
        }
    }

    public void StartShooting(IGun<GunRuntime> gun)
    {
        if (_gunStates.TryGetValue(gun, out GunState t))
        {
            t.IsShooting = true;
        }
        else
        {
            Debug.Log($"{gun}が見つかりません");
        }
    }

    public void StopShooting(IGun<GunRuntime> gun)
    {
        if (_gunStates.TryGetValue(gun, out GunState t))
        {
            t.IsShooting = false;
        }
        else
        {
            Debug.Log($"{gun}が見つかりません");
        }
    }

    //銃がGunServiceに問い合わせるのは「intervalかどうか」だけ
    public void OnUpdate()
    {

    }

    private class GunState
    {
        public IGun<GunRuntime> Gun;
        public float LastShotTime;
        public bool IsShooting;
    }
}
