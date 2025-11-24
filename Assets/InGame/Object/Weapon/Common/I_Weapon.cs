using System.Collections;
using System.Collections.Generic;
using Game.Data;
using NUnit.Framework.Constraints;
using UnityEngine;

public interface IWeapon<TRuntimeData> : IItem<TRuntimeData> where TRuntimeData : AWeaponRuntimeDataBase
{
    WeaponType WeaponType { get; }
    RuntimeAnimatorController WeaponAnim { get; } //武器専用AnimatorOverrideController

    void Initialize(TRuntimeData weaponRuntimeData);
    void AttackStart();
    void AttackProcess();
    void AttackEnd();
}
