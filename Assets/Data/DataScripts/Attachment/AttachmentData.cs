using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/Attachment", fileName = "NewAttachment")]
public abstract class AttachmentData : ItemData
{
    public AttachmentType attachmentType;
    public GunStatModifier[] modifiers; // 小さな構造体で修正値を定義
    public ItemVisualData visual; // optional 見た目SO
}