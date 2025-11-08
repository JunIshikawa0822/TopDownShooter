using System;
using System.Collections.Generic;

//「どの部位に」「どんなタイプのアタッチメントを装着できるか」を定義するテンプレート。
[Serializable]
public class AttachmentSlotData
{
    public string slotID;//"MainGrip"など
    public AttachmentType slotType;//Grip等
    public List<string> allowedTags;//違う方法で許可を表せる（optional）
    public AttachmentData defaultAttachmentData;//null許容
}
