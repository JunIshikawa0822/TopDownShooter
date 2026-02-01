using System.Collections.Generic;
using Game.Data;

public class AttachmentSlot 
{

    private readonly AttachmentType _equippableType;
    private AttachmentRuntime _equipAttachment;

    public AttachmentRuntime Current => _equipAttachment;//装着されているアタッチメント（nullなら空き）
    public AttachmentType EquippableType => _equippableType;

    public AttachmentSlot(AttachmentType equippableType)
    {
        _equippableType = equippableType;
    }

    public bool IsEquippable(AttachmentRuntime attachment)
    {
        return _equippableType == attachment.AttachmentType;
    }

    public void SetAttachment(AttachmentRuntime attachment)
    {
        _equipAttachment = attachment;
    }

    public AttachmentRuntime RemoveAttachment()
    {
        AttachmentRuntime oldAttachment = _equipAttachment;
        _equipAttachment = null;
        return oldAttachment;
    }
}