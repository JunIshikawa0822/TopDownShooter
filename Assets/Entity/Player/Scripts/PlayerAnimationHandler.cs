using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationHandler : MonoBehaviour, IAnimationHandler
{
    [SerializeField] private Animator _animator;
    [SerializeField] private float _dampTime = 0.1f;
    private float _currentSpeed;

    public void OnMove(float speed)
    {
        _currentSpeed = speed;
        _animator.SetFloat("Speed", _currentSpeed);
    }
}
