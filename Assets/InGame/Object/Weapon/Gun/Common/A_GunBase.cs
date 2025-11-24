using Game.Data;
using UnityEngine;

public abstract class AGunBase<TRuntimeData> : AWeaponBase<TRuntimeData>, IGun<TRuntimeData>
    where TRuntimeData : GunRuntimeData
{
    [SerializeField] private Transform _muzzleTrans;
    private IObjectPool<Bullet> _bulletPool;

    protected float _lastShotTime = 0f;

    public override void Initialize(TRuntimeData gunData)
    {
        base.Initialize(gunData);
        _muzzleTrans.position = RuntimeData.GunBaseData.BulletSpawnPos;
    }

    public virtual void SetBulletPool(IObjectPool<Bullet> bulletPool)
    {
        _bulletPool = bulletPool;
    }

    public virtual void Reload()
    {
        //インベントリから新しい対応するマガジンを探し出して、セットする
    }

    public override void AttackStart()
    {
        if (IntervalWait()) return;
        
        Shot();
    }

    public override void AttackProcess()
    {

    }

    public override void AttackEnd()
    {

    }

    protected virtual void Shot()
    {
        if (_bulletPool == null) return;
        Bullet bullet = _bulletPool.GetFromPool();

        if (bullet == null)
        {
            Debug.Log("キャスト無理");
            return;
        }

        AttachmentRuntimeData_Magazine magazine = RuntimeData.GetAttachmentByType(AttachmentType.Magazine) as AttachmentRuntimeData_Magazine;
        if (magazine == null) return;

        //弾が消費できたらtrue, できないならfalse
        if (!magazine.ConsumeBullet()) return;

        bullet.transform.position = _muzzleTrans.position;

        bullet.Init(magazine.LoadedAmmoData, _muzzleTrans.forward, RuntimeData.GunBaseData.BulletVelocity);
    }
    
    //trueならinterval中、falseならそうでない
    protected bool IntervalWait()
    {
        float interval = 1f / RuntimeData.FireRate;
        if (Time.time - _lastShotTime < interval) return true;

        _lastShotTime = Time.time;
        return false;
    }
}
