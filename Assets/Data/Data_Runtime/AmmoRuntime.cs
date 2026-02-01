using UnityEngine;

namespace Game.Data
{
    //Ammoをインベントリで扱うためのフォーマット
    public class AmmoRuntime : AItemRuntimeBase
    {
        public AmmoData AmmoData => ItemData as AmmoData;
        public AmmoType AmmoType => AmmoData?.AmmoType ?? AmmoType.None;
        public AmmoRuntime(AmmoData ammoData, int initialCount = 1) : base(ammoData, initialCount)
        {
            
        }

        protected override AItemRuntimeBase CreateCopy(int initialCount)
        {
            return new AmmoRuntime(AmmoData, initialCount);
        }
    }
}

