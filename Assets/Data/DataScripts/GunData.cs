using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "MyGame/Weapon/GunData")]
    public class GunData : WeaponData
    {
        [Header("装填設定")]
        [SerializeField] private AmmoType _targetAmmo;//必須：どの弾丸を使えるか
        [SerializeField] private bool _isInternalMagazine = false;//マガジンを必要としないタイプか
        [SerializeField] private int _internalCapacity;//基本装填数

        [Header("射撃ロジック")]
        [SerializeField] private FireType _fireType;
        [SerializeField] private int _burstCount = 3;
        [SerializeField] private int _simulNum = 1;//同時発射数（ショットガンなど）
        [SerializeField] private float _rpm; //rounds per minute

        [Header("反動・精度")]
        [SerializeField] private float _velocity;//射出速度
        [SerializeField] private float _horizontalRecoil;//水平反動値 左右のブレの絶対値
        [SerializeField] private float _verticalRecoil;//垂直反動値 上下のブレの絶対値　下向きの制御はコード側で行う
        
        //照準の中心から「半径何メートル（または角度何度）の円の中に弾が飛ぶか」という円の大きさ（半径）を表す。
        // 単位は一般的にDegree
        [SerializeField] private float _shotSpread;
        [SerializeField] private float _baseScatter;//拡散のランダム
        [SerializeField] private float _scatterIncriment;//連射時の拡散増加量
        [SerializeField] private float _maxScatter = 3.0f;//連射時の拡散増加量上限

        [Header("取り回し")]
        [SerializeField] private float _reloadTime;
        [SerializeField] private float _ergonomics; //取り回し（ADSへの移行時間に関わる数値）

        [SerializeField] private float _maxRange;//有効射程距離

        public AmmoType TargetAmmo => _targetAmmo;
        public bool IsInternalMagazine => _isInternalMagazine;
        public int InternalCapacity => _internalCapacity;

        public FireType FireType => _fireType;
        public int BurstCount => _burstCount;
        public int SimulNum => _simulNum;
        //射撃間隔（秒）
        public float FireInterval => _rpm > 0 ? 60f / _rpm : 0.1f;

        public float Velocity => _velocity > 0 ? _velocity : 10;
        public float HorizontalRecoil => _horizontalRecoil;
        public float VerticalRecoil => _verticalRecoil;
        public float ShotSpread => _shotSpread;
        public float BaseScatter => _baseScatter;
        public float ScatterIncriment => _scatterIncriment;
        public float MaxScatter => _maxScatter;

        public float ReloadTime => _reloadTime;
        public float Ergonomics => _ergonomics;
        public float MaxRange => _maxRange > 5 ? _maxRange : 5;
    }
}

public enum GunStatType
{
    FireRate,
    HorizontalRecoil,
    VerticalRecoil,
    Accuracy,
    BulletVelocity,
    MaxMagazineCapacity,
    MaxRange,
    Count//enumの末尾に配置、ループや配列長取得用
}