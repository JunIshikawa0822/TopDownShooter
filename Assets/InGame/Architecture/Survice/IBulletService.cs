using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IBulletService
{
    public Bullet GetBullet();
    public void BulletInit(Vector3 pos, Vector3 dir, float range, float speed, LayerMask mask, Action<RaycastHit> onHit);
}
