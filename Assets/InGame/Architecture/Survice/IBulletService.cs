using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IBulletService
{
    public void BulletInit(AmmoData ammoData, Vector3 pos, Vector3 dir, float range, float speed, LayerMask mask);
}
