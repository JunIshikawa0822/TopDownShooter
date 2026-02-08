using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "MyGame/Attachment/MagazineData")]
    public class AttachmentData_Magazine : AttachmentData
    {
        [Header("マガジン設定")]
        [Min(1), SerializeField] private uint _capacity;
        [SerializeField] private AmmoType _targetAmmo; // 弾薬口径

        public uint Capacity => _capacity;
        public AmmoType TargetAmmo => _targetAmmo;
    }
}


