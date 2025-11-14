using UnityEngine;

public class Rifle : APooledObject
{
    [SerializeField] private AnimatorOverrideController _animatorOverride;

    public WeaponType WeaponType => WeaponType.Rifle;
    public RuntimeAnimatorController AnimatorOverrideController => _animatorOverride;

    public void AttackStart()
    {

    }

    public void AttackProcess()
    {

    }

    public void AttackEnd()
    {

    }
    
    public void Reload()
    {
        
    }
}