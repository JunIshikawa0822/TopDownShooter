using System.Collections;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

public abstract class AWeaponBase<TRuntimeData> : AWeaponBase, IWeapon<TRuntimeData> where TRuntimeData : AWeaponRuntimeDataBase
{
    protected TRuntimeData _weaponRuntimeData;

    public TRuntimeData RuntimeData => _weaponRuntimeData;
    public WeaponType WeaponType => RuntimeData.WeaponType;
    public RuntimeAnimatorController WeaponAnim => RuntimeData.WeaponBaseData.WeaponAnim;

    public virtual void Initialize(TRuntimeData weaponRuntimeData)
    {
        _weaponRuntimeData = weaponRuntimeData;
    }
}
