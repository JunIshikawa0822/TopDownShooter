using System.Collections;
using System.Collections.Generic;
using Game.Items;
using UnityEngine;

public interface IGun<out TRuntimeData> : IWeapon<TRuntimeData> where TRuntimeData : GunRuntimeData
{
    public void Initialize(GunRuntimeData gunData, IObjectPool objectPool);
    public void Reload();
}