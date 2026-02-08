using System;
using UnityEngine;

public class Factory_BulletVisual : IObjectFactory<BulletVisual>
{
    private BulletVisual _bulletVisualPrefab;
    public Factory_BulletVisual(BulletVisual bulletVisualPrefab)
    {
        _bulletVisualPrefab = bulletVisualPrefab;
    }

    public BulletVisual ObjectInstantiate()
    {
        BulletVisual newBullet = GameObject.Instantiate(_bulletVisualPrefab);

        return newBullet;
    }
}
