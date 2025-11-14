using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    [CreateAssetMenu(menuName = "Game/Item/GunData")]
    public class GunData : WeaponData
    {
        [Header("射撃設定")]
        [SerializeField] private FireMode _fireMode;//射撃タイプ（セミ・フル・バーストなど）
        [SerializeField] private float _fireRate;//発射間隔（1秒間に何発打てるか）
        [SerializeField] private float _recoil;//反動値
        [SerializeField] private float _accuracy;//命中精度
        [SerializeField] private float _bulletVelocity;//弾速

        [Header("アタッチメント設定")]
        //どんなアタッチメントがつけられるかを定義
        [SerializeField] private List<AttachmentSlotData> _ableAttachmentSlots;

        public FireMode FireMode => _fireMode;
        public float FireRate => _fireRate;
        public float Recoil => _recoil;
        public float Accuracy => _accuracy;
        public float BulletVelocity => _bulletVelocity;
        public IReadOnlyList<AttachmentSlotData> AbleAttachmentSlots => _ableAttachmentSlots;
    }

    public enum GunStatType
    {
        FireRate,
        Recoil,
        Accuracy,
        BulletVelocity,
        MaxMagazineCapacity,

        Count//enumの末尾に配置、ループや配列長取得用
    }
}

