using System.Collections;
using System.Collections.Generic;
using Game.Data;
using NUnit.Framework.Constraints;
using UnityEngine;

public interface IWeapon<out TRuntimeData> : IItem<TRuntimeData> where TRuntimeData : AWeaponRuntimeBase
{
    WeaponType WeaponType { get; }
    RuntimeAnimatorController WeaponAnim { get; } //武器専用AnimatorOverrideController
    void AttackStart(bool isAttackSupportInput);
    void AttackProcess(bool isAttackSupportInput);
    void AttackEnd(bool isAttackSupportInput);
}
