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

    // ここで一度だけ型チェックを行い、安全なら本来の初期化を呼ぶ
    public override void Initialize(AWeaponRuntimeDataBase data)
    {
        // ここで一度だけ型チェックを行い、安全なら本来の初期化を呼ぶ
        if (data is TRuntimeData specificData)
        {
            Initialize(specificData);
        }
        else
        {
            Debug.LogError($"型が合いません！ {typeof(TRuntimeData)} が必要です。");
        }

        base.Initialize(data);
    }

    protected virtual void WeaponSetUp(TRuntimeData weaponRuntimeData)
    {
        _weaponRuntimeData = weaponRuntimeData;
    }
}
