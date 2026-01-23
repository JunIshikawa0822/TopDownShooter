using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/Attachment", fileName = "NewAttachment")]
public abstract class GunAttachmentData : ItemData
{
    [SerializeField] private AttachmentType _attachmentType;
    [SerializeField] private GunStatModifier[] _modifiers;
    public AttachmentType AttachmentType => _attachmentType;
    public GunStatModifier[] Modifiers => _modifiers; // 小さな構造体で修正値を定義
}