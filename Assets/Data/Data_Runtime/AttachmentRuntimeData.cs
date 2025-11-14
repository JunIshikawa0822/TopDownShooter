using UnityEngine;
using System.Collections.Generic;

namespace Game.Items
{
    public class AttachmentRuntimeData : AItemRuntimeDataBase
    {
        public AttachmentData AttachmentData => BaseData as AttachmentData;
        public AttachmentType AttachmentType => AttachmentData.attachmentType;
        public IReadOnlyList<GunStatModifier> Modifiers => AttachmentData.modifiers;

        public AttachmentRuntimeData(AttachmentData baseData, int initialCount = 1) : base(baseData, initialCount)
        {
            
        }
    }
}