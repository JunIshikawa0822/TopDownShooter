using System;
using System.Collections.Generic;

[Serializable]
public class AttachmentSlot
{
    public string slotId;//"MainGrip"　など
    public AttachmentType slotType;//Grip等
    public List<string> allowedTags;//違う方法で許可を表せる（optional）
    public AttachmentData defaultAttachment;//null許容
}
