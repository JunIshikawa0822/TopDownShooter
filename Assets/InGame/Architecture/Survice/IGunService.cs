using System.Collections;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

public interface IGunService
{
    public bool CanShoot(IGun<GunRuntime> gun);
    public bool IsBulletConsume { get; }
    public void RecordShotTime(IGun<GunRuntime> gun);
    public void StartShooting(IGun<GunRuntime> gun);
    public void StopShooting(IGun<GunRuntime> gun);
}
