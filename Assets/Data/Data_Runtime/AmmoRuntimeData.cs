using UnityEngine;

namespace Game.Items
{
    //Ammoをインベントリで扱うためのフォーマット
    public class AmmoRuntimeData : ItemRuntimeDataBase
    {
        public AmmoData AmmoBaseData => BaseData as AmmoData;
        public AmmoCaliberType Caliber => AmmoBaseData?.Caliber ?? AmmoCaliberType.None;
        public float Damage => AmmoBaseData?.Damage ?? 0f;
        public float PenetrationPower => AmmoBaseData?.PenetrationPower ?? 0f;
        public AmmoRuntimeData(AmmoData baseData, int initialCount = 1) : base(baseData, initialCount)
        {
            
        }
    }
}

