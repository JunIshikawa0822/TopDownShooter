using Game.Data;
using UnityEngine;

public class PlayerController : A_Entity, IWeaponHandler
{
    //現在装備中の武器（3Dオブジェクト）
    private AWeaponBase _currentWeapon;
    [SerializeField] private Transform _righthand;
    //プレイヤーの移動速度倍率
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _rotateSpeed = 500f;
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
        _velocity = move * _moveSpeed * _playerTime * Time.deltaTime;
        _rigidbody.linearVelocity = _velocity;

        // 直接参照型でアニメーションに通知
        _animationHandler?.OnMove(this);

        // --- 拡張時に Event 化したい場合 ---
        // OnMoved?.Invoke(this);
    }

    public void Rotate(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction - transform.position);
        transform.eulerAngles = transform.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, transform.eulerAngles.y, _rotateSpeed * _playerTime * Time.deltaTime);
        
        if(_currentWeapon == null) return;
        transform.rotation = 
        _righthand.rotation = transform.rotation;
    }

    public void Equip(AWeaponBase weapon)
    {
        _currentWeapon = weapon;
        _currentWeapon.transform.SetParent(this._righthand);
        _currentWeapon.transform.SetPositionAndRotation(_righthand.position, _righthand.rotation);
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
