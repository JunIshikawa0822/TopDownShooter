using System;
using UnityEngine;

namespace Game.Data
{
    public class MeleeRuntime : AWeaponRuntimeBase
    {
        public MeleeData MeleeData => ItemData as MeleeData;
        public MeleeRuntime(MeleeData meleeData, int initialCount = 1, Guid? runtimeGuid = null) : base(meleeData, initialCount, runtimeGuid)
        {

        }
    }
}