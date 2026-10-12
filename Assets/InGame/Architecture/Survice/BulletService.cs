using System.Collections.Generic;
using UnityEngine;
using Game.Data;
using System;
public class BulletService : IBulletService, IOnFixedUpdate, IOnUpdate
{
    //弾オブジェクトを飛ばす場合に用いる
    //private HashSet<Bullet> _activeBullets = new();
    //計算で弾オブジェクトを飛ばす場合に用いる
    //private List<BulletStepData> _bullets = new();
    //計算で弾オブジェクトを飛ばす場合に用いる　新版
    private List<BulletData> _bulletDatas = new();
    //弾オブジェクトを飛ばす場合に用いる
    //private IObjectPool<Bullet> _bulletPool;
    //計算で弾オブジェクトを飛ばす場合に用いる
    private IObjectPool<BulletVisual> _bulletVisualPool;
    public bool IsActiveForFixedUpdate => true;
    public bool IsActiveForUpdate => true;

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
        _bulletVisualPool = bulletVisualPool;
    }

    public void OnFixedUpdate()
    {

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

        for (int i = _bulletDatas.Count - 1; i >= 0; i--)
        {
            BulletData b = _bulletDatas[i];
            BulletVisual b_s = b.BulletVisual;

            Vector3 interpolated = Vector3.Lerp(b.PrevPos, b.Pos, Mathf.Clamp01(t));
            b_s.transform.position = interpolated;
        }
    }

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