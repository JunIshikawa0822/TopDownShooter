using System;
using UnityEngine;

public class Factory_Bullet: IFactory<Bullet>
{
    private Bullet _bulletPrefab;
    public Factory_Bullet(Bullet bulletPrefab)
    {
        _bulletPrefab = bulletPrefab;
    }

    public Bullet ObjectInstantiate()
    {
        Bullet newBullet = GameObject.Instantiate(_bulletPrefab);

        return newBullet;
    }
}
