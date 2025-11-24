using UnityEngine;

namespace Game.Data
{
    public class MeleeRuntimeData : AWeaponRuntimeDataBase
    {
        private readonly float[] _baseStats;
        private readonly float[] _currentStats;
        public MeleeRuntimeData(MeleeData baseData, int initialCount = 1) : base(baseData, initialCount)
        {
            
        }
    }
}