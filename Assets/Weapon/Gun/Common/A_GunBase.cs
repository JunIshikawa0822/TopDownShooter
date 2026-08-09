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

    public override void AttackStart(bool isAiming)
    {
        if (GunRuntime.FireType == FireType.Burst) BurstFire(isAiming).Forget();
        if (GunRuntime.FireType == FireType.FullAuto || GunRuntime.FireType == FireType.Semi)
        {
            Debug.Log(CanShoot());
            if (!CanShoot()) return;

            _gunService.StartShooting(this);

            SetBullet(isAiming);
            GunRuntime.RecordShotTime();
            InvokeMuzzleFlash().Forget();
        }
    }

    protected virtual async UniTaskVoid BurstFire(bool isAiming)
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

            SetBullet(isAiming);
            GunRuntime.RecordShotTime();
            InvokeMuzzleFlash().Forget();

            if (i < count - 1)
            {
                await UniTask.Delay(intervalMs, cancellationToken: ct);
            }
        }

        _gunService.StopShooting(this);
    }

    //TODO: 連射によって弾の拡散や反動を増加する計算を追加する
    public override void AttackProcess(bool isAiming)
    {
        if (GunRuntime.FireType == FireType.Semi || GunRuntime.FireType == FireType.Burst) return;
        if (GunRuntime.FireType == FireType.FullAuto)
        {
            if (!CanShoot()) return;

            SetBullet(isAiming);
            GunRuntime.RecordShotTime();
            InvokeMuzzleFlash().Forget();
        }
    }

    protected virtual bool CanShoot()
    {
        if (!TryClipCheck()) return false;
        if (GunRuntime.IsIntervalShooting) return false;
        if (!GunRuntime.CanConsume(_gunService.IsBulletConsume)) return false;
        return true;
    }

    public override void AttackEnd(bool isAiming)
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
    //TODO: 弾のブレを考慮する計算を追加する
    // protected void SetBullet(Vector3 dir)
    // {
    //     Vector3 destinationPoint = GetDestination(_muzzleTrans.position, dir);
    //     float range = Vector3.Distance(_muzzleTrans.position, destinationPoint);

    //     _bulletSurvice.BulletInit
    //     (
    //         GunRuntime.LoadAmmoData,
    //         _muzzleTrans.position,
    //         dir,
    //         Mathf.Min(range, GunRuntime.MaxRange),
    //         GunRuntime.Velocity,
    //         _collideLayerMask
    //     );
    // }

    protected void SetBullet(bool isAiming)
    {
        float startAngle = (GunRuntime.SimulNum > 1) ? -GunRuntime.ShotSpread : 0;
        startAngle *= isAiming ? 0.5f : 1f;

        float angleStep = (GunRuntime.SimulNum > 1) ? GunRuntime.ShotSpread / (GunRuntime.SimulNum - 1) : 0;

        for (int i = 0; i < GunRuntime.SimulNum; ++i)
        {
            //扇状の配置角度
            float baseAngle = startAngle + (angleStep * i);

            //TODO: ランダムな値は本当に0.5でいいのか
            //TODO: 武器やその他プレイヤーの状態によるScatterの増加も考慮するとよい
            float randomOffset = UnityEngine.Random.Range(-0.5f, 0.5f) * GunRuntime.CurrentScatter;

            //合計の回転角
            float finalAngle = baseAngle + randomOffset;

            //muzzle.forward（基準方向）をY軸中心に回転
            Vector3 bulletDir = (Quaternion.Euler(0, finalAngle, 0) * _muzzleTrans.forward).normalized;

            Vector3 destinationPoint = GetDestination(_muzzleTrans.position, bulletDir);
            float range = Vector3.Distance(_muzzleTrans.position, destinationPoint);

            _bulletSurvice.BulletInit
            (
                GunRuntime.LoadAmmoData,
                _muzzleTrans.position,
                bulletDir,
                Mathf.Min(range, GunRuntime.MaxRange),
                GunRuntime.Velocity,
                _collideLayerMask
            );
        }

        GunRuntime.IncrimentScatter();
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
