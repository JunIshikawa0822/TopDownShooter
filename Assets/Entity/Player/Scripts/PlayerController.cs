using UnityEngine;

public class PlayerController : A_Entity
{
    private Rigidbody _playerRigidbody;
    private IAnimationHandler _animationHandler;

    //プレイヤーの時間倍率（プレイヤーのみスローモーションにするなど用)
    private float _playerTime = 1;

    //プレイヤーの移動速度倍率
    private float _playerMoveSpeed = 1;

    public override void OnSetUp(IAnimationHandler animationHandler)
    {
        _animationHandler = animationHandler;
        _playerRigidbody = GetComponent<Rigidbody>();
    }

    public override void Move(Vector2 direction)
    {
        _playerRigidbody.linearVelocity = new Vector3(direction.x, 0, direction.y) * _playerMoveSpeed * _playerTime;
    }
}
