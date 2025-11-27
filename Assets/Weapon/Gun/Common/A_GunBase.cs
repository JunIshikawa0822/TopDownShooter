using System;
using Game.Data;
using UnityEngine;
using UnityEngine.VFX;
using System.Collections;

public abstract class AGunBase<TRuntimeData> : AWeaponBase<TRuntimeData>, IGun<TRuntimeData>
    where TRuntimeData : GunRuntimeData
{
    [SerializeField] protected LayerMask _collideLayerMask;
    [SerializeField] protected Transform _muzzleTrans;
    [SerializeField] protected VisualEffect _muzzleFlash;
    [SerializeField] protected Light _muzzleLight;
    protected IGunService _gunService;
    protected IBulletService _bulletSurvice;

    [SerializeField] protected float _checkClipDist_Forward;
    [SerializeField] protected float _checkClipDist_Backward;

    public GunRuntimeData GunRuntimeData => RuntimeData;

    public override void Initialize(TRuntimeData gunData)
    {
        base.Initialize(gunData);
        Debug.Log("Initialized");
        Debug.Log(RuntimeData.GunBaseData.BulletSpawnPos);
        _muzzleTrans.localPosition = RuntimeData.GunBaseData.BulletSpawnPos;

        _muzzleLight.enabled = false;
        _muzzleFlash.Reinit();
    }

    public virtual void SetGunSurvice(IGunService gunService)
    {
        _gunService = gunService;
    }

    public virtual void SetBulletSurvice(IBulletService bulletService)
    {
        _bulletSurvice = bulletService;
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
        Bullet bullet = _bulletSurvice.GetBullet();
        if (bullet == null)return;

        bullet.transform.position = _muzzleTrans.position;
        bullet.transform.SetPositionAndRotation(_muzzleTrans.position, _muzzleTrans.rotation);
        bullet.Init(_weaponRuntimeData.CurrentAmmoData, _muzzleTrans.forward, _weaponRuntimeData.BulletVelocity, _weaponRuntimeData.MaxRange);
    }

    protected Vector3 GetDestination(Vector3 startPos, Vector3 dir)
    {
        Vector3 destination = dir * _weaponRuntimeData.MaxRange;
        if (Physics.Raycast(_muzzleTrans.position, _muzzleTrans.forward, out RaycastHit staticHit, _weaponRuntimeData.MaxRange, _collideLayerMask, QueryTriggerInteraction.Ignore))
        {
            destination = staticHit.point;
        }

        return destination;
    }

    protected void SetRayBullet()
    {
        Vector3 destination = GetDestination(_muzzleTrans.position, _muzzleTrans.forward);
        float range = Vector3.Distance(_muzzleTrans.position, destination);

        _bulletSurvice.BulletInit
        (
            _muzzleTrans.position,
            _muzzleTrans.forward,
            range,
            _weaponRuntimeData.BulletVelocity,
            _collideLayerMask,
            DamageInvoke
        );
    }

    protected void DamageInvoke(RaycastHit opponent)
    {
        if(opponent.transform.TryGetComponent<IDamagable>(out IDamagable  damagable))
        {
            
        }
    }

    protected IEnumerator InvokeMuzzleFlash()
    {
        // コルーチンが開始された瞬間にライトをONにするため、VFXと同時になる
        _muzzleLight.enabled = true; // ★ライト点灯★
        _muzzleFlash.SendEvent("OnPlay");
        Debug.Log("ライト");

        // LIGHT_DURATION (例: 0.05秒) 待機
        yield return new WaitForSeconds(0.15f);

        _muzzleLight.enabled = false;
        _muzzleFlash.SendEvent("OnStop");
        _muzzleFlash.Reinit();
    }
}
