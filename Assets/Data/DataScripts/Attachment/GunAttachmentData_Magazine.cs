using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "MyGame/Attachment/MagazineData")]
    public class GunAttachmentData_Magazine : GunAttachmentData
    {
        [Header("マガジン設定")]
        [Min(1), SerializeField] private uint _capacity;
        [SerializeField] private uint _defaultAmmoNum;
        [SerializeField] private AmmoCaliberType _compatibleCaliber; // 弾薬口径
        [SerializeField] private float _reloadTime;

        public uint Capacity => _capacity;
        public uint DefaultAmmoNum => _defaultAmmoNum;
        public AmmoCaliberType CompatibleCaliber => _compatibleCaliber;
        public float ReloadTime => _reloadTime;
    }
}


