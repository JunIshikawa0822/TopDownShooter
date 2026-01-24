using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "MyGame/Weapon/GunData")]
    public class GunData : WeaponData
    {
        [Header("射撃設定")]
        [SerializeField] private FireType _fireType;//射撃タイプ（セミ・フル・バーストなど）
        [SerializeField] private int _burstCount;//バーストの場合、何連か
        //TODO: RPMで表す方法に移行したいな〜
        [SerializeField] private int _fireRate;//発射間隔（1秒間に何発打てるか）
        [SerializeField] private float _horizontalRecoil;//水平反動値
        [SerializeField] private float _verticalRecoil;//垂直反動値
        [SerializeField] private float _accuracy;//命中精度？？？
        [SerializeField] private float _bulletVelocity;//弾速
        [SerializeField] private float _maxRange;//射程距離

        [Header("アタッチメント設定")]
        //どんなアタッチメントがつけられるかを定義
        [SerializeField] private GunAttachmentSlotData[] _ableAttachmentSlots;

        [Header("マガジンがつかない武器")]
        //何発装填できるかを定義
        [SerializeField] private int _internalAmmoMax;

        public FireType FireType => _fireType;
        public int BurstCount => _burstCount;
        public int FireRate => _fireRate;
        public float HorizontalRecoil => _horizontalRecoil;
        public float Accuracy => _accuracy;
        public float BulletVelocity => _bulletVelocity;
        public float MaxRange => _maxRange;
        public IReadOnlyList<GunAttachmentSlotData> AbleAttachmentSlots => _ableAttachmentSlots;
        public int InternalAmmoMax => _internalAmmoMax;
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