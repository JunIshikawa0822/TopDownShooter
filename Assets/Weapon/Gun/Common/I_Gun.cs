using System.Collections;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;
using System;

public interface IGun<out TRuntime> : IWeapon<TRuntime> where TRuntime : GunRuntime
{
    public GunRuntime GunRuntime { get; }
    //public void SetBulletPool(IObjectPool<Bullet> bullet);
    public void SetGunSurvice(IGunService gunService);
    public void SetBulletSurvice(IBulletService bulletService);
    public void Reload();
}