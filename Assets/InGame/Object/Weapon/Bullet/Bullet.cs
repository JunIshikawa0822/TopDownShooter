using UnityEngine;

public class Bullet : APooledObject, IBullet, IOnUpdate
{
    private AmmoData _ammoData;
    private Transform _bulletTransform;
    private Vector3 _bulletPrePos;
    private float _bulletDistance;

    private float _timeScale = 1f;

    private Vector3 _direction;
    private float _speed;

    #region プロパティ
    public AmmoData Data => _ammoData;
    public float Damage => Data.Damage;
    public float PenetrationPower => Data.PenetrationPower;
    public AmmoCaliberType CaliberType => Data.Caliber;
    #endregion

    public virtual void Init(AmmoData ammoData, Vector3 direction, float speed)
    {
        _ammoData = ammoData;
        _bulletTransform = this.transform;

        _bulletPrePos = _bulletTransform.position;

        _direction = direction.normalized;
        _speed = speed;

        _bulletDistance = 0f;
    }

    public virtual void OnUpdate()
    {
        //弾の移動（Time.deltaTimeではなく独自管理）
        Vector3 move = _direction * _speed * _timeScale;
        _bulletTransform.position += move;

        //移動距離計算
        float movedDistance = (_bulletTransform.position - _bulletPrePos).magnitude;
        _bulletDistance += movedDistance;

        //当たり判定
        if (Physics.Raycast(_bulletPrePos, _direction, out RaycastHit hit, movedDistance))
        {
            _bulletPrePos = _bulletTransform.position;
            // ここでダメージ処理や貫通処理
            OnHit(hit);

            //もし貫通するならこれは呼ぶ場所を考えるべき
            ReturnToPool();
            return;
        }

        _bulletPrePos = _bulletTransform.position;

        //最大距離でプールに返却
        if (_bulletDistance > Data.MaxLifeDistance)
        {
            ReturnToPool();
        }
    }
    
    private void OnHit(RaycastHit hit)
    {
        //ダメージ計算・エフェクト生成
        //貫通を実装するなら
    }
}
