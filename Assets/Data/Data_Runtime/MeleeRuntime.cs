using UnityEngine;

namespace Game.Data
{
    public class MeleeRuntime : AWeaponRuntimeBase
    {
        public MeleeData MeleeData =>  ItemData as MeleeData;
        public MeleeRuntime(MeleeData meleeData, int initialCount = 1) : base(meleeData, initialCount)
        {
            
        }
        protected override AItemRuntimeBase CreateCopy(int initialCount)
        {
            return new MeleeRuntime(MeleeData, initialCount);
        }
    }
}