using Game.Items;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class AGunBase : APooledObject, IGun<GunRuntimeData>
{
    [SerializeField] private Transform _muzzleTrans;
    private IObjectPool _bulletPool;
    private GunRuntimeData _gunRuntimeData;

    protected float _lastShotTime = 0f;

    #region プロパティ
    public GunRuntimeData RuntimeData => _gunRuntimeData;
    public WeaponType WeaponType => RuntimeData.WeaponType;
    public RuntimeAnimatorController WeaponAnim => RuntimeData.GunBaseData.WeaponAnim;
    #endregion

    public virtual void Initialize(GunRuntimeData gunData, IObjectPool objectPool)
    {
        _gunRuntimeData = gunData;
        _bulletPool = objectPool;
    }

    public virtual void Reload()
    {
        //インベントリから新しい対応するマガジンを探し出して、セットする
    }

    public virtual void AttackStart()
    {
        if (IntervalWait()) return;
        
        Shot();
    }

    public virtual void AttackProcess()
    {

    }

    public virtual void AttackEnd()
    {

    }

    protected virtual void Shot()
    {
        if (_bulletPool == null) return;
        Bullet bullet = _bulletPool.GetFromPool() as Bullet;

        if (bullet == null)
        {
            Debug.Log("キャスト無理");
            return;
        }

        AttachmentRuntimeData_Magazine magazine = RuntimeData.GetAttachmentByType(AttachmentType.Magazine) as AttachmentRuntimeData_Magazine;
        if (magazine == null) return;

        //弾が消費できたらtrue, できないならfalse
        if (!magazine.ConsumeBullet()) return;

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
