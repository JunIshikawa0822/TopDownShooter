using UnityEngine;

namespace Game.Items
{
    [CreateAssetMenu(menuName = "Game/Attachment/MagazineData")]
    public class AttachmentData_Magazine : AttachmentData
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


