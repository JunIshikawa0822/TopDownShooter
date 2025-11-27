using System.Collections.Generic;
using UnityEngine;
using System;
public class BulletService : IBulletService, IOnFixedUpdate, IOnUpdate
{
    private HashSet<Bullet> _activeBullets = new();
    private List<BulletStepData> _bullets = new();
    private List<BulletVisual> _bulletVisuals = new();
    private IObjectPool<Bullet> _bulletPool;
    private IObjectPool<BulletVisual> _bulletVisualPool;
    public bool IsActiveForFixedUpdate => true;
    public bool IsActiveForUpdate => true;

    #region Rayでやる場合

    #endregion

    struct BulletStepData 
    {
        public Vector3 Pos;
        public Vector3 Dir;
        public float DistRemain;
        public float Speed;
        public LayerMask CollideMask; // Gunが渡す（もしくはOwner経由で渡す）
        public Action<RaycastHit> OnHit; // Gunが渡す
    }

    public BulletService(IObjectPool<Bullet> bulletPool, IObjectPool<BulletVisual> bulletVisualPool)
    {
        _bulletPool = bulletPool;
        _bulletVisualPool = bulletVisualPool;
    }

    public void OnFixedUpdate()
    {
        foreach(Bullet bullet in _activeBullets)
        {
            if(!bullet.IsActiveForFixedUpdate)return;
            bullet.OnFixedUpdate(); // 移動・寿命・当たり判定
        }

        for (int i = _bullets.Count - 1; i >= 0; i--)
        {
            BulletStepData b = _bullets[i];
            float step = b.Speed * Time.fixedDeltaTime;

            if (Physics.Raycast(b.Pos, b.Dir, out RaycastHit hit, step, b.CollideMask))
            {
                b.OnHit?.Invoke(hit);
                BulletExpired(i);
                continue;
            }

            b.Pos += b.Dir * step;
            b.DistRemain -= step;

            if (b.DistRemain <= 0f) BulletExpired(i);
            else _bullets[i] = b;
        }
    }

    public void OnUpdate()
    {
        for (int i = _bullets.Count - 1; i >= 0; i--)
        {
            BulletStepData b = _bullets[i];
            BulletVisual b_s = _bulletVisuals[i];

            b_s.transform.position = b.Pos;
        }
    }
    
    public Bullet GetBullet()
    {
        Bullet bullet = _bulletPool.GetFromPool();
        if (bullet == null) return null;

        _activeBullets.Add(bullet); // HashSet なので重複自動排除
        return bullet;
    }

    public void BulletInit(Vector3 startPos, Vector3 dir, float range, float speed, LayerMask mask, Action<RaycastHit> onHit)
    {
        _bullets.Add(new BulletStepData
        {
            Pos = startPos,
            Dir = dir,
            Speed = speed,
            DistRemain = range,
            CollideMask = mask,
            OnHit = onHit
        });

        BulletVisual bulletVisual = _bulletVisualPool.GetFromPool();
        bulletVisual.transform.position = startPos;
        bulletVisual.transform.forward = dir;
        bulletVisual.Active();
        _bulletVisuals.Add(bulletVisual);
    }

    private void BulletExpired(int i)
    {
        _bullets.RemoveAt(i);
        
        _bulletVisuals[i].Deactive();
        _bulletVisuals[i].ReturnToPool();
        _bulletVisuals.RemoveAt(i);
    }
}