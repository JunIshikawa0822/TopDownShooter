using Game.Data;
using UnityEngine;
using System.Collections.Generic;

public class GunService : IOnUpdate, IGunService
{
    public bool IsActiveForUpdate => true;
    private Dictionary<IGun<GunRuntimeData>, GunState> _gunStates = new();

    public void RegisterGun(IGun<GunRuntimeData> gun)
    {
        GunState state = new(){Gun = gun, IsShooting = false};
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
        if(_gunStates.TryGetValue(gun, out GunState t))
        {
            t.LastShotTime = Time.time;
        }
        else
        {
            Debug.Log($"{gun}が見つかりません");
        }
    }

    public void StartShooting(IGun<GunRuntimeData> gun)
    {
        if(_gunStates.TryGetValue(gun, out GunState t))
        {
            t.IsShooting = true;
        }
        else
        {
            Debug.Log($"{gun}が見つかりません");
        }
    }

    public void StopShooting(IGun<GunRuntimeData> gun)
    {
        if(_gunStates.TryGetValue(gun, out GunState t))
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
        public IGun<GunRuntimeData> Gun;
        public float LastShotTime;
        public bool IsShooting;
    }
}
