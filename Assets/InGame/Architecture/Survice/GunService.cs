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

    // public bool CanShoot(IGun<GunRuntime> gun)
    // {
    //     // GunRuntime自体が時間管理するようになったため、シンプルに委譲する形、もしくは不要になる
    //     // 一旦、GunRuntimeの情報を使って判定するように修正
    //     bool canShoot = !gun.GunRuntime.IsIntervalShooting;

    //     return canShoot;
    // }

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

    public void OnUpdate()
    {

    }

    private class GunState
    {
        public IGun<GunRuntime> Gun;
        //public float LastShotTime;
        public bool IsShooting;
    }
}
