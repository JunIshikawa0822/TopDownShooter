using System;
using Game.Data;
using UnityEngine;
using UnityEngine.VFX;
using System.Collections;
using Cysharp.Threading.Tasks;
using System.Threading;

public abstract class AGunBase<TRuntime> : AWeaponBase<TRuntime>, IGun<TRuntime>
    where TRuntime : GunRuntime
{
    private const float MUZZLE_FLASH_DURATION = 0.05f;
    [SerializeField] protected LayerMask _collideLayerMask;
    [SerializeField] protected Transform _muzzleTrans;
    [SerializeField] protected VisualEffect _muzzleFlash;
    [SerializeField] protected Light _muzzleLight;
    protected IGunService _gunService;
    protected IBulletService _bulletSurvice;

    [SerializeField] protected float _checkClipDist_Forward;
    [SerializeField] protected float _checkClipDist_Backward;

    public GunRuntime GunRuntime => Runtime;

    public override void Initialize(AWeaponRuntimeBase weaponData)
    {
        base.Initialize(weaponData);
    }

    protected override void WeaponSetUp(TRuntime weaponRuntimeData)
    {
        base.WeaponSetUp(weaponRuntimeData);

        _muzzleTrans.localPosition = GetAnchor(WeaponAnchorType.Muzzle).localPosition;

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
        if (GunRuntime.FireType == FireType.Burst) BurstFire().Forget();
        if (GunRuntime.FireType == FireType.FullAuto || GunRuntime.FireType == FireType.Semi)
        {
            if (!CanShoot()) return;

            _gunService.StartShooting(this);

            SetBullet(_muzzleTrans.forward);
            _gunService.RecordShotTime(this);
            InvokeMuzzleFlash().Forget();
        }
    }

    protected virtual async UniTaskVoid BurstFire()
    {
        CancellationToken ct = this.GetCancellationTokenOnDestroy();

        int count = GunRuntime.BurstCount;
        int intervalMs = (int)(GunRuntime.FireInterval * 1000);

        for (int i = 0; i < count; i++)
        {
            if (!CanShoot()) break;
            if (i == 0)
            {
                _gunService.StartShooting(this);
            }

            SetBullet(_muzzleTrans.forward);
            _gunService.RecordShotTime(this);
            InvokeMuzzleFlash().Forget();

            if (i < count - 1)
            {
                await UniTask.Delay(intervalMs, cancellationToken: ct);
            }
        }

        _gunService.StopShooting(this);
    }

    public override void AttackProcess()
    {
        if (GunRuntime.FireType == FireType.Semi || GunRuntime.FireType == FireType.Burst) return;
        if (GunRuntime.FireType == FireType.FullAuto)
        {
            if (!CanShoot()) return;

            SetBullet(_muzzleTrans.forward);
            _gunService.RecordShotTime(this);
            InvokeMuzzleFlash().Forget();
        }
    }

    protected virtual bool CanShoot()
    {
        if (!TryClipCheck()) return false;
        if (!_gunService.CanShoot(this)) return false;
        if (!GunRuntime.CanConsume(_gunService.IsBulletConsume)) return false;
        return true;
    }

    public override void AttackEnd()
    {
        _gunService.StopShooting(this);
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
    // protected virtual void SpawnBullet()
    // {
    //     Bullet bullet = _bulletSurvice.GetBullet();
    //     if (bullet == null)return;

    //     bullet.transform.position = _muzzleTrans.position;
    //     bullet.transform.SetPositionAndRotation(_muzzleTrans.position, _muzzleTrans.rotation);
    //     bullet.Init(_weaponRuntimeData.CurrentAmmoData, _muzzleTrans.forward, _weaponRuntimeData.BulletVelocity, _weaponRuntimeData.MaxRange);
    // }

    //計算で弾を飛ばすのに必要
    protected Vector3 GetDestination(Vector3 startPos, Vector3 dir)
    {
        Vector3 destination = dir * GunRuntime.GunData.MaxRange;
        if (Physics.Raycast(_muzzleTrans.position, _muzzleTrans.forward, out RaycastHit staticHit, GunRuntime.GunData.MaxRange, _collideLayerMask, QueryTriggerInteraction.Ignore))
        {
            destination = staticHit.point;
        }

        return destination;
    }

    //計算で弾を飛ばすのに必要
    protected void SetBullet(Vector3 dir)
    {
        Vector3 destinationPoint = GetDestination(_muzzleTrans.position, dir);
        float range = Vector3.Distance(_muzzleTrans.position, destinationPoint);

        _bulletSurvice.BulletInit
        (
            GunRuntime.LoadAmmoData,
            _muzzleTrans.position,
            dir,
            Mathf.Min(range, GunRuntime.MaxRange),
            GunRuntime.Velocity,
            _collideLayerMask
        );
    }

    protected virtual async UniTaskVoid InvokeMuzzleFlash()
    {
        // コルーチンが開始された瞬間にライトをONにするため、VFXと同時になる
        _muzzleLight.enabled = true; // ★ライト点灯★
        _muzzleFlash.SendEvent("OnPlay");
        //        Debug.Log("ライト");

        // LIGHT_DURATION (例: 0.05秒) 待機
        await UniTask.Delay((int)(MUZZLE_FLASH_DURATION * 1000));

        _muzzleLight.enabled = false;
        _muzzleFlash.SendEvent("OnStop");
        _muzzleFlash.Reinit();
    }
}
