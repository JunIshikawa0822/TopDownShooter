using UnityEngine;
using System;

namespace Game.Data
{
    public class GunAttachmentSlotRuntimeData
    {
        private GunAttachmentSlotData _slotData;
        private string _slotID;
        private readonly AttachmentType _slotType;
        private GunAttachmentRuntimeData _equippedAttachment;

        public event Action onChanged;

        public GunAttachmentSlotData BaseData => _slotData;
        public string SlotId => _slotID;
        public AttachmentType SlotType => _slotType;
        public GunAttachmentRuntimeData EquippedAttachment => _equippedAttachment;

        public GunAttachmentSlotRuntimeData(GunAttachmentSlotData slodData, string slotId, AttachmentType slotType, GunAttachmentRuntimeData defaultAttachment = null)
        {
            _slotData = slodData;
            _slotID = slotId;
            _slotType = slotType;
            _equippedAttachment = defaultAttachment;
        }
        
        public bool Equip(GunAttachmentRuntimeData attachment)
        {
            if (!CanAttach(attachment)) return false;

            _equippedAttachment = attachment;
            onChanged?.Invoke();
            return true;
        }

        //外したアタッチメントを返す
        public GunAttachmentRuntimeData UnEquip()
        {
            GunAttachmentRuntimeData removed = _equippedAttachment;
            _equippedAttachment = null;
            onChanged?.Invoke();
            return removed;
        }

        //すでに埋まっている場合は外してから出ないと装着不可
        public bool CanAttach(GunAttachmentRuntimeData attachment)
        {
            if (attachment == null) return false;
            if (_equippedAttachment != null) return false;
            if (attachment.AttachmentData.AttachmentType != _slotType)
                return false;

            return true;
        }
    }
}
