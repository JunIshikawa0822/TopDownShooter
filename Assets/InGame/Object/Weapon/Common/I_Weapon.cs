using System.Collections;
using System.Collections.Generic;
using Game.Items;
using UnityEngine;

public interface IWeapon<out TRuntimeData> : IItem<TRuntimeData> where TRuntimeData : AWeaponRuntimeDataBase
{
    WeaponType WeaponType { get; }
    RuntimeAnimatorController WeaponAnim { get; } //武器専用AnimatorOverrideController

    void AttackStart();
    void AttackProcess();
    void AttackEnd();
}
