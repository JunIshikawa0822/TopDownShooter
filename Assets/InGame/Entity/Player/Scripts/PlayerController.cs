using Game.Items;
using UnityEngine;

public class PlayerController : A_Entity, IWeaponHandler
{
    //現在装備中の武器（3Dオブジェクト）
    private IWeapon<AWeaponRuntimeDataBase> _currentWeapon;
    //プレイヤーの移動速度倍率
    [SerializeField] private float _playerMoveSpeed = 5f;
    //プレイヤーの時間倍率（プレイヤーのみスローモーションにするなど用)
    [SerializeField] private float _playerTime = 1f;
    private Rigidbody _rigidbody;
    private Vector3 _velocity;
    private Vector2 _direction;
    private IAnimationHandler _animationHandler;

#region Property
    public override Vector3 Velocity => _velocity;
    public override Vector2 MoveDirection => _direction;
    public override float TimeScale => _playerTime;
#endregion
    public override void OnSetUp(IAnimationHandler animationHandler)
    {
        _animationHandler = animationHandler;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void Move(Vector2 direction)
    {
        _direction = direction;

        // オブジェクトの前方向・右方向を基準にワールド座標に変換
        //Vector3 move = transform.forward * _direction.y + transform.right * _direction.x;
        Vector3 move = new Vector3(_direction.x, 0, _direction.y);

        //_velocity = new Vector3(_direction.x, 0, _direction.y) * _playerMoveSpeed * _playerTime;
        _velocity = move * _playerMoveSpeed * _playerTime;
        _rigidbody.linearVelocity = _velocity;

        // 直接参照型でアニメーションに通知
        _animationHandler?.OnMove(this);

        // --- 拡張時に Event 化したい場合 ---
        // OnMoved?.Invoke(this);
    }

    public void Rotate(Vector2 direction)
    {

    }

    public void Equip(IWeapon<AWeaponRuntimeDataBase> weapon)
    {
        _currentWeapon = weapon;
    }

    public void AttackStart()
    {
        _currentWeapon?.AttackStart();
    }

    public void AttackProcess()
    {
        _currentWeapon?.AttackProcess();
    }
    
    public void AttckEnd()
    {
        _currentWeapon?.AttackEnd();
    }
}
