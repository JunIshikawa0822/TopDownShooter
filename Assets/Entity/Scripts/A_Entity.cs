using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public abstract class AEntity : MonoBehaviour, IDamageable
{
    public abstract Vector3 Velocity { get; }
    public abstract Vector3 MoveDirection { get; }
    public abstract float TimeScale { get; }

    public abstract void OnSetUp(IAnimationHandler animationHandler = null);
    public abstract void Move(Vector3 direction, bool isSprinting = false);
    public abstract void TakeDamage(DamageInfo damageInfo);
}