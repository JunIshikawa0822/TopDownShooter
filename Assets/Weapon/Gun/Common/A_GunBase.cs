using System;
using Game.Data;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public abstract class AGunBase<TRuntimeData> : AWeaponBase<TRuntimeData>, IGun<TRuntimeData>
    where TRuntimeData : GunRuntimeData
{
    [SerializeField] protected Transform _muzzleTrans;
    protected IGunService _gunService;

    [SerializeField] protected float _checkClipDist_Forward;
    [SerializeField] protected float _checkClipDist_Backward;

    public GunRuntimeData GunRuntimeData => RuntimeData;

    public override void Initialize(TRuntimeData gunData)
    {
        base.Initialize(gunData);
        _muzzleTrans.position = RuntimeData.GunBaseData.BulletSpawnPos;
    }

    public virtual void Reload()
    {
        //インベントリから新しい対応するマガジンを探し出して、セットする
    }

    public override void AttackStart()
    {

    }

    public override void AttackProcess()
    {
        
    }

    public override void AttackEnd()
    {

    }

    protected bool TryConsumeBullets()
    {
        return _weaponRuntimeData.TryConsumeRuntimeBullets();
    }

    protected bool TryClipCheck()
    {
        // クリッピングチェック
        if (Physics.Raycast(_muzzleTrans.position, _muzzleTrans.forward, _checkClipDist_Forward))
            return false;

        if (Physics.Raycast(_muzzleTrans.position, -_muzzleTrans.forward, _checkClipDist_Backward))
            return false;

        return true;
    }


    //マガジンで給弾するかどうか　trueで給弾、falseは直接
    protected virtual void SpawnBullet()
    {
        Bullet bullet = _gunService.GetBullet();
        if (bullet == null)return;

        bullet.transform.position = _muzzleTrans.position;
        bullet.Init(_weaponRuntimeData.CurrentAmmoData, _muzzleTrans.forward, _weaponRuntimeData.BulletVelocity);
    }
}
