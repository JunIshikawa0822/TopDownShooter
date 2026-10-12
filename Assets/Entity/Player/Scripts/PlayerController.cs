using Game.Data;
using UnityEngine;

public class PlayerController : AEntity, IWeaponHandler
{
    //現在装備中の武器（3Dオブジェクト）
    private AWeaponBase _currentWeapon;
    [SerializeField] private Transform _righthand;
    //攻撃の水平基準点（銃の弾丸を水平に飛ばすため）
    [SerializeField] private Transform _attackBaseTrans;
    //プレイヤーの移動速度倍率
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _sprintMultiplier = 2;
    [SerializeField] private float _rotateSpeed = 500f;
    [SerializeField] private float _weaponRotationSpeed = 20;
    //プレイヤーの時間倍率（プレイヤーのみスローモーションにするなど用)
    [SerializeField] private float _playerTime = 1f;

    [Header("Weaponの回転に関するルール")]
    [SerializeField] private float _minAngle = 20f;    // 近距離での最小許容角度 (2m以内)
    [SerializeField] private float _maxAngle = 50f;    // 遠距離での最大許容角度 (10m以上)
    [SerializeField] private float _minDistance = 2f;  // 近距離と見なす閾値
    [SerializeField] private float _maxDistance = 10f; // 遠距離と見なす閾値
    private Rigidbody _rigidbody;
    private Vector3 _velocity;
    private Vector3 _localMoveDirection;
    private IAnimationHandler _animationHandler;

    public Transform AttackBaseTrans => _attackBaseTrans;

#region Property
    public override Vector3 Velocity => _velocity;
    public override Vector3 MoveDirection => _localMoveDirection;
    public override float TimeScale => _playerTime;
#endregion
    public override void OnSetUp(IAnimationHandler animationHandler)
    {
        _animationHandler = animationHandler;
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void Move(Vector3 direction, bool isSprinting)
    {
        _localMoveDirection = GetLocalMovementAxes(direction);

        float currentSpeed = isSprinting ? _moveSpeed * _sprintMultiplier : _moveSpeed;

        _velocity = direction * currentSpeed * _playerTime * Time.fixedDeltaTime;
        _rigidbody.linearVelocity = _velocity;

        // 直接参照型でアニメーションに通知
        _animationHandler?.OnMove(this);

        // --- 拡張時に Event 化したい場合 ---
        // OnMoved?.Invoke(this);
    }

    public void Rotate(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction - transform.position);
        transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, lookRotation.eulerAngles.y, _rotateSpeed * _playerTime * Time.deltaTime);
        
        if(_currentWeapon == null) return;
        Vector3 rawDirectionToTarget = direction - _righthand.position; 
        float rawDistance = rawDirectionToTarget.magnitude;

        Vector3 stableDirectionForRotation;

        if (rawDistance < _minDistance)
        {
            // 距離が近すぎる（デッドゾーン内）なら、
            // マウスの方向は無視して、「プレイヤーの体の正面」をターゲット方向とする。
            stableDirectionForRotation = transform.forward;
        }
        else
        {
            // 距離が十分あるなら、マウスの方向を採用する
            stableDirectionForRotation = rawDirectionToTarget;
        }

        // 射角チェック
        Vector3 flatDirectionFromCursor = new Vector3(stableDirectionForRotation.x, 0, stableDirectionForRotation.z).normalized;
        float verticalAngle = Vector3.Angle(stableDirectionForRotation, flatDirectionFromCursor); 

        // 距離に応じた制限角度の計算
        float t = Mathf.InverseLerp(_minDistance, _maxDistance, rawDistance);
        float currentMaxAngle = Mathf.Lerp(_minAngle, _maxAngle, t);

        Quaternion targetRotation;
        
        if(verticalAngle >= currentMaxAngle) 
        {
            //射角外なら、体の正面を向く（前回の修正）
            targetRotation = Quaternion.LookRotation(transform.forward);
        }
        else
        {
            //射角内なら、さっき決めた「安定化された方向」を向く
            targetRotation = Quaternion.LookRotation(stableDirectionForRotation);
        }

        //スムージング
        _righthand.rotation = Quaternion.Slerp(_righthand.rotation, targetRotation, Time.deltaTime * _playerTime * _weaponRotationSpeed);
    }

    public void Equip(AWeaponBase weapon)
    {
        _currentWeapon = weapon;
        _currentWeapon.transform.SetParent(this._righthand);
        _currentWeapon.transform.SetPositionAndRotation(_righthand.position, _righthand.rotation);
    }

    public override void TakeDamage(DamageInfo damageInfo)
    {
        
    }

    //プレイヤーの向きに対してどの方向に移動しているか
    private Vector3 GetLocalMovementAxes(Vector3 worldMovementVector)
    {
        // 移動入力がない場合は停止
        if (worldMovementVector.sqrMagnitude < 0.01f)
        {
            return Vector3.zero;
        }

        //プレイヤーのローカル空間に変換
        Vector3 localMove = transform.InverseTransformDirection(worldMovementVector);

        //ローカル軸の成分を抽出
        float forwardSpeed = localMove.z; //プレイヤーから見て前後の速度
        float sidewaysSpeed = localMove.x; //プレイヤーから見て左右の速度
        
        //ローカル軸ベクトルを作成し、Vector3として返す
        //X軸に左右速度、Z軸に前後速度を格納し、Y軸は0とする
        Vector3 localAxes3D = new Vector3(sidewaysSpeed, 0f, forwardSpeed);

        return localAxes3D; 
    }
}
