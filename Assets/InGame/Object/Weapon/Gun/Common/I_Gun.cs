using System.Collections;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

public interface IGun<TRuntimeData> : IWeapon<TRuntimeData> where TRuntimeData : GunRuntimeData
{
    public void SetBulletPool(IObjectPool<Bullet> bullet);
    public void Reload();
}