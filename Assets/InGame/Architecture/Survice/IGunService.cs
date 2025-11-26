using System.Collections;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

public interface IGunService
{
    public bool CanShoot(IGun<GunRuntimeData> gun);
    public void RecordShotTime(IGun<GunRuntimeData> gun);
    public Bullet GetBullet();
}
