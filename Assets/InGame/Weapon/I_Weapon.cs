using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeapon
{
    WeaponType WeaponType { get; }
    RuntimeAnimatorController AnimatorOverrideController { get; } //武器専用AnimatorOverrideController

    void AttackStart();
    void AttackProcess();
    void AttackEnd();
}
