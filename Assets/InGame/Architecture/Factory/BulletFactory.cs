using System;
using UnityEngine;

public class Factory_Bullet: IFactory<Bullet>
{
    private readonly Action<IOnUpdate> _registerAction;
    private Bullet _bulletPrefab;
    public Factory_Bullet(Bullet bulletPrefab, Action<IOnUpdate> action)
    {
        _bulletPrefab = bulletPrefab;
        _registerAction = action;
    }

    public Bullet ObjectInstantiate()
    {
        Bullet newBullet = GameObject.Instantiate(_bulletPrefab);
        _registerAction?.Invoke(newBullet);

        return newBullet;
    }
}
