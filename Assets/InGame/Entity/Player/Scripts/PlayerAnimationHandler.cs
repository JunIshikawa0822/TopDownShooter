using UnityEditor.Experimental.GraphView;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationHandler : MonoBehaviour, IAnimationHandler
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _dampTime = 0.01f;
    private float _currentSpeed;

    public void OnMove(A_Entity entity)
    {
        float deltaTime = Time.deltaTime * entity.TimeScale;
        _currentSpeed = Mathf.Lerp(_currentSpeed, entity.Velocity.magnitude, deltaTime / _dampTime);

        if (_currentSpeed < 0.01f) _currentSpeed = 0f;
        _animator.SetFloat("Speed", _currentSpeed);

        _animator.SetFloat("MoveDir_X", entity.MoveDirection.x);
        _animator.SetFloat("MoveDir_Y", entity.MoveDirection.y);
    }

    // private void OnAnimatorIK(int layerIndex)
    // {
    //     if (!_animator) return;

    //     _animator.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1f);
    //     _animator.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1f);
    //     _animator.SetIKPosition(AvatarIKGoal.LeftFoot, leftFootTarget.position);
    //     _animator.SetIKRotation(AvatarIKGoal.LeftFoot, leftFootTarget.rotation);

    //     _animator.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1f);
    //     _animator.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1f);
    //     _animator.SetIKPosition(AvatarIKGoal.RightFoot, rightFootTarget.position);
    //     _animator.SetIKRotation(AvatarIKGoal.RightFoot, rightFootTarget.rotation);
    // }

}
