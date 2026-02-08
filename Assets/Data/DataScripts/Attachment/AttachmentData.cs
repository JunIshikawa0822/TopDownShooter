using UnityEngine;
namespace Game.Data
{
    [CreateAssetMenu(menuName = "MyGame/Attachment", fileName = "NewAttachment")]
    public abstract class AttachmentData : ItemData
    {
        [SerializeField] private AttachmentType _attachmentType;
        [SerializeField] private StatModifier[] _modifiers;
        [Header("装備可能なアタッチメント")]
        [SerializeField] private AttachmentType[] _equippableTypes;
        public AttachmentType AttachmentType => _attachmentType;
        public StatModifier[] Modifiers => _modifiers; //修正値を定義
        public AttachmentType[] EquippableTypes => _equippableTypes;
    }
}