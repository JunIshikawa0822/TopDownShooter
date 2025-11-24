using Game.Data;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationHandler : MonoBehaviour, IAnimationHandler
{
    [SerializeField] private Animator _animator;
    // カテゴリごとのベースAnimatorController
    [SerializeField] private RuntimeAnimatorController _noneAnimatorControllerBase;
    [SerializeField] private RuntimeAnimatorController _gunAnimatorControllerBase;
    [SerializeField] private RuntimeAnimatorController _meleeAnimatorControllerBase;
    [SerializeField] private float _dampTime = 0.01f;
    private float _currentSpeed;

    public void OnMove(A_Entity entity)
    {
        float deltaTime = Time.deltaTime * entity.TimeScale;
        _currentSpeed = Mathf.Lerp(_currentSpeed, entity.Velocity.magnitude, deltaTime / _dampTime);

        if (_currentSpeed < 0.01f) _currentSpeed = 0f;
        _animator.SetFloat("Speed", _currentSpeed);

        _animator.SetFloat("MoveDir_X", entity.MoveDirection.x);
        _animator.SetFloat("MoveDir_Y", entity.MoveDirection.z);
    }

    public void OnWeaponEquipped(IWeapon<AWeaponRuntimeDataBase> weapon)
    {
        //武器の種類に応じて基本Controllerを切り替え
        if (weapon.WeaponType == WeaponType.None)
            _animator.runtimeAnimatorController = _noneAnimatorControllerBase;
        else if (weapon.WeaponType == WeaponType.Melee)
            _animator.runtimeAnimatorController = _meleeAnimatorControllerBase;
        else
            _animator.runtimeAnimatorController = _gunAnimatorControllerBase;

        //武器固有のOverrideControllerを適用
        if (weapon.WeaponAnim != null)
            _animator.runtimeAnimatorController = weapon.WeaponAnim;
    }
}