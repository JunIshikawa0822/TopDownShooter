using System.Collections.Generic;
using UnityEngine;
using System;
public class BulletService : IBulletService, IOnFixedUpdate, IOnUpdate
{
    //弾オブジェクトを飛ばす場合に用いる
    //private HashSet<Bullet> _activeBullets = new();
    //計算で弾オブジェクトを飛ばす場合に用いる
    //private List<BulletStepData> _bullets = new();
    //計算で弾オブジェクトを飛ばす場合に用いる　新版
    private List<BulletData> _bulletDatas = new();
    private List<BulletVisual> _bulletVisuals = new();
    //弾オブジェクトを飛ばす場合に用いる
    //private IObjectPool<Bullet> _bulletPool;
    //計算で弾オブジェクトを飛ばす場合に用いる
    private IObjectPool<BulletVisual> _bulletVisualPool;
    public bool IsActiveForFixedUpdate => true;
    public bool IsActiveForUpdate => true;

    #region Rayでやる場合

    #endregion

    // struct BulletStepData
    // {
    //     public Vector3 Pos;
    //     public Vector3 PrevPos;
    //     public Vector3 Dir;
    //     public float DistRemain;
    //     public float Speed;
    //     public LayerMask CollideMask; // Gunが渡す（もしくはOwner経由で渡す）
    //     public Action<RaycastHit> OnHit; // Gunが渡す
    // }

    struct BulletData
    {
        public Vector3 Pos;
        public Vector3 PrevPos;
        public Vector3 Dir;
        public float DistRemain;
        public float Speed;
        public LayerMask CollideMask; // Gunが渡す（もしくはOwner経由で渡す）
        public BulletVisual BulletVisual;
        public AmmoData AmmoData;
    }

    public BulletService(IObjectPool<BulletVisual> bulletVisualPool)
    {
        //_bulletPool = bulletPool;
        _bulletVisualPool = bulletVisualPool;
    }

    public void OnFixedUpdate()
    {
        //実際に弾オブジェクトを飛ばす場合に用いる計算
        // foreach(Bullet bullet in _activeBullets)
        // {
        //     if(!bullet.IsActiveForFixedUpdate)return;
        //     bullet.OnFixedUpdate(); // 移動・寿命・当たり判定
        // }

        //計算のみで弾オブジェクトを飛ばす場合に用いる計算
        // for (int i = _bullets.Count - 1; i >= 0; i--)
        // {
        //     BulletStepData b = _bullets[i];
        //     float step = b.Speed * Time.fixedDeltaTime;

        //     b.PrevPos = b.Pos;

        //     if (Physics.Raycast(b.Pos, b.Dir, out RaycastHit hit, step, b.CollideMask))
        //     {
        //         b.OnHit?.Invoke(hit);
        //         BulletExpired(i);
        //         continue;
        //     }

        //     b.Pos += b.Dir * step;
        //     b.DistRemain -= step;

        //     if (b.DistRemain <= 0f) BulletExpired(i);
        //     else _bullets[i] = b;
        // }

        //計算のみで弾オブジェクトを飛ばす場合に用いる計算 新版
        for (int i = _bulletDatas.Count - 1; i >= 0; i--)
        {
            BulletData b = _bulletDatas[i];
            float step = b.Speed * Time.fixedDeltaTime;

            b.PrevPos = b.Pos;

            if (Physics.Raycast(b.Pos, b.Dir, out RaycastHit hit, step, b.CollideMask))
            {
                
                BulletExpire(i);
                continue;
            }

            b.Pos += b.Dir * step;
            b.DistRemain -= step;

            if (b.DistRemain <= 0f) BulletExpire(i);
            //値型なので更新
            else _bulletDatas[i] = b;
        }
    }

    public void OnUpdate()
    {
        float t = (Time.time - Time.fixedTime) / Time.fixedDeltaTime;

        // for (int i = _bullets.Count - 1; i >= 0; i--)
        // {
        //     BulletStepData b = _bullets[i];
        //     BulletVisual b_s = _bulletVisuals[i];

        //     Vector3 interpolated = Vector3.Lerp(b.PrevPos, b.Pos, Mathf.Clamp01(t));
        //     b_s.transform.position = interpolated;
        // }

        for (int i = _bulletDatas.Count - 1; i >= 0; i--)
        {
            BulletData b = _bulletDatas[i];
            BulletVisual b_s = b.BulletVisual;

            Vector3 interpolated = Vector3.Lerp(b.PrevPos, b.Pos, Mathf.Clamp01(t));
            b_s.transform.position = interpolated;
        }
    }
    
    //実際に弾オブジェクトを飛ばす場合に用いる計算
    // public Bullet GetBullet()
    // {
    //     Bullet bullet = _bulletPool.GetFromPool();
    //     if (bullet == null) return null;

    //     _activeBullets.Add(bullet); // HashSet なので重複自動排除
    //     return bullet;
    // }

    //計算のみで弾オブジェクトを飛ばす場合に用いる計算
    // public void BulletInit(Vector3 startPos, Vector3 dir, float range, float speed, LayerMask mask, Action<RaycastHit> onHit)
    // {
    //     _bullets.Add(new BulletStepData
    //     {
    //         Pos = startPos,
    //         PrevPos = startPos,
    //         Dir = dir,
    //         Speed = speed,
    //         DistRemain = range,
    //         CollideMask = mask,
    //         OnHit = onHit
    //     });

    //     BulletVisual bulletVisual = _bulletVisualPool.GetFromPool();
    //     bulletVisual.transform.position = startPos;
    //     bulletVisual.transform.forward = dir;
    //     bulletVisual.Active();
    //     _bulletVisuals.Add(bulletVisual);
    // }

    //こっち使ったほうが安全 新版
    public void BulletInit(AmmoData ammoData, Vector3 startPos, Vector3 dir, float range, float speed, LayerMask mask)
    {
        BulletVisual bulletVisual = _bulletVisualPool.GetFromPool();

        _bulletDatas.Add(new BulletData
        {
            Pos = startPos,
            PrevPos = startPos,
            Dir = dir,
            Speed = speed,
            DistRemain = range,
            CollideMask = mask,
            BulletVisual = bulletVisual,
            AmmoData = ammoData
        });
        
        bulletVisual.transform.position = startPos;
        bulletVisual.transform.forward = dir;
        bulletVisual.Active();
    }

    // private void BulletExpired(int i)
    // {
    //     _bullets.RemoveAt(i);
        
    //     _bulletVisuals[i].Deactive();
    //     _bulletVisuals[i].ReturnToPool();
    //     _bulletVisuals.RemoveAt(i);
    // }

    //こっち使ったほうが安全 新版
    private void BulletExpire(int i)
    {
        BulletData b = _bulletDatas[i];
        b.BulletVisual.Deactive();
        b.BulletVisual.ReturnToPool();
        
        _bulletDatas.RemoveAt(i);
        // _bulletVisualsリストを別途持たず、BulletStepDataから消せば不整合が起きない
    }
}