using System.Collections.Generic;
using UnityEngine;

namespace Game.Items
{
    [CreateAssetMenu(menuName = "Game/Item/GunData")]
    public class GunData : WeaponData
    {
        [Header("射撃設定")]
        [SerializeField] private FireMode _fireMode;//射撃タイプ（セミ・フル・バーストなど）
        [SerializeField] private float _fireRate;//発射間隔
        [SerializeField] private float _recoil;//反動値
        [SerializeField] private float _accuracy;//命中精度
        [SerializeField] private float _bulletVelocity;//弾速
        [SerializeField] private int _defaultMagazineSize;//標準マガジン容量

        [Header("アタッチメント設定")]
        [SerializeField] private List<AttachmentSlot> _attachmentSlots;
        // 例：スコープスロット、マガジンスロット、グリップスロットなど

        public FireMode FireMode => _fireMode;
        public float FireRate => _fireRate;
        public float Recoil => _recoil;
        public float Accuracy => _accuracy;
        public float BulletVelocity => _bulletVelocity;
        public int DefaultMagazineSize => _defaultMagazineSize;
        public IReadOnlyList<AttachmentSlot> AttachmentSlots => _attachmentSlots;
    }
}

