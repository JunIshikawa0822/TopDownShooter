using UnityEngine;
using System;

namespace Game.Items
{
    public class AttachmentSlotRuntimeData
    {
        private string _slotID;
        private readonly AttachmentType _slotType;
        private AttachmentRuntimeData _equippedAttachment;

        public event Action onChanged;
        public string SlotId => _slotID;
        public AttachmentType SlotType => _slotType;
        public AttachmentRuntimeData EquippedAttachment => _equippedAttachment;

        public AttachmentSlotRuntimeData(string slotId, AttachmentType slotType, AttachmentRuntimeData defaultAttachment = null)
        {
            _slotID = slotId;
            _slotType = slotType;
            _equippedAttachment = defaultAttachment;
        }
        
        public bool Equip(AttachmentRuntimeData attachment)
        {
            if (!CanAttach(attachment)) return false;

            _equippedAttachment = attachment;
            onChanged?.Invoke();
            return true;
        }

        //外したアタッチメントを返す
        public AttachmentRuntimeData UnEquip()
        {
            AttachmentRuntimeData removed = _equippedAttachment;
            _equippedAttachment = null;
            onChanged?.Invoke();
            return removed;
        }

        //すでに埋まっている場合は外してから出ないと装着不可
        public bool CanAttach(AttachmentRuntimeData attachment)
        {
            if (attachment == null) return false;
            if (_equippedAttachment != null) return false;
            if (attachment.AttachmentData.attachmentType != _slotType)
                return false;

            return true;
        }
    }
}
