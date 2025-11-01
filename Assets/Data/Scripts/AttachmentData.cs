using UnityEngine;

[CreateAssetMenu(menuName = "Game/Attachment", fileName = "NewAttachment")]
public class AttachmentData : ScriptableObject
{
    public string id;
    public AttachmentType attachmentType;
    public StatModifier[] modifiers; // 小さな構造体で修正値を定義
    public ItemVisualData visual; // optional 見た目SO
}