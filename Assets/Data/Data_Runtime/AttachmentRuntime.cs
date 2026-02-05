using UnityEngine;
using System.Collections.Generic;
using System;

namespace Game.Data
{
    public class AttachmentRuntime : AItemRuntimeBase, IStatModifierProvider
    {
        //このアタッチメントを装着することで増えるスロット
        protected AttachmentSlot[] _subSlots;
        public AttachmentData AttachmentData => ItemData as AttachmentData;
        public AttachmentType AttachmentType => AttachmentData.AttachmentType;
        public IReadOnlyList<AttachmentSlot> SubSlots => _subSlots;
        public AttachmentRuntime(AttachmentData attachmentData, int initialCount = 1, Guid? runtimeGuid = null) : base(attachmentData, initialCount, runtimeGuid)
        {
            // Data側で定義された追加スロットを生成（例：マウントレールなら追加のRailスロット等）
            if (attachmentData.EquippableTypes != null)
            {
                _subSlots = new AttachmentSlot[attachmentData.EquippableTypes.Length];
                for (int i = 0; i < _subSlots.Length; ++i)
                {
                    _subSlots[i] = new AttachmentSlot(attachmentData.EquippableTypes[i]);
                }
            }
        }

        public IEnumerable<StatModifier> GetModifiers() => AttachmentData.Modifiers;
    }
}