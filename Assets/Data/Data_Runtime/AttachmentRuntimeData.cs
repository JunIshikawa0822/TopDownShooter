using UnityEngine;
using System.Collections.Generic;

namespace Game.Data
{
    public class GunAttachmentRuntimeData : AItemRuntimeDataBase
    {
        public GunAttachmentData AttachmentData => BaseData as GunAttachmentData;
        public AttachmentType AttachmentType => AttachmentData.AttachmentType;
        public IReadOnlyList<GunStatModifier> Modifiers => AttachmentData.Modifiers;

        public GunAttachmentRuntimeData(GunAttachmentData baseData, int initialCount = 1) : base(baseData, initialCount)
        {
            
        }
    }
}