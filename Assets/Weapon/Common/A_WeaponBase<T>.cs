using System.Collections;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

public abstract class AWeaponBase<TRuntime> : AWeaponBase, IWeapon<TRuntime> where TRuntime : AWeaponRuntimeBase
{
    protected TRuntime _weaponRuntimeBase;
    public TRuntime Runtime => _weaponRuntimeBase;
    public WeaponType WeaponType => Runtime.WeaponType;
    public RuntimeAnimatorController WeaponAnim => Runtime.WeaponData.WeaponAnim;

    // ここで一度だけ型チェックを行い、安全なら本来の初期化を呼ぶ
    public override void Initialize(AWeaponRuntimeBase data)
    {
        // ここで一度だけ型チェックを行い、安全なら武器の初期化を呼ぶ
        if (data is TRuntime specificData)
        {
            WeaponSetUp(specificData);
        }
        else
        {
            Debug.LogError($"型が合いません！ {typeof(TRuntime)} が必要です。");
        }

        base.Initialize(data);
    }

    protected virtual void WeaponSetUp(TRuntime weaponRuntimeData)
    {
        _weaponRuntimeBase = weaponRuntimeData;
    }
}
