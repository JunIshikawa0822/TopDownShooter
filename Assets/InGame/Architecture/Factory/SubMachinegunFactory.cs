using System;
using UnityEngine;

public class Factory_SubMachinegun : IFactory<AWeaponBase>
{
    //private IObjectPool<Bullet> _bulletPool;
    private SubMachinegun _subMachinegunPrefab;
    public Factory_SubMachinegun(SubMachinegun subMachinegunPrefab/*, IObjectPool<Bullet> bulletPool*/)
    {
        _subMachinegunPrefab = subMachinegunPrefab;
        //_bulletPool = bulletPool;
    }

    public AWeaponBase ObjectInstantiate()
    {
        SubMachinegun newSubMachinegun = GameObject.Instantiate(_subMachinegunPrefab);
        //newSubMachinegun.SetBulletPool(_bulletPool);

        return newSubMachinegun;
    }
}
