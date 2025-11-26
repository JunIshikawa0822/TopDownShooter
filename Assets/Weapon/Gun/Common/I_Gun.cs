using System.Collections;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;
using System;

public interface IGun<TRuntimeData> : IWeapon<TRuntimeData> where TRuntimeData : GunRuntimeData
{
    public GunRuntimeData GunRuntimeData{get;}
    //public void SetBulletPool(IObjectPool<Bullet> bullet);
    public void Reload();
}