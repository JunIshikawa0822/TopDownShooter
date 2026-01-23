using UnityEngine;
using System.Collections.Generic;

namespace Game.Data
{
    public class GunAttachmentRuntimeData_Magazine : GunAttachmentRuntimeData
    {
        private uint _capacity;
        private uint _remaining;
        private readonly AmmoCaliberType _supportedCaliber;
        private AmmoData _loadedAmmoData;

        public GunAttachmentData_Magazine AttachmentData_Magazine => BaseData as GunAttachmentData_Magazine;
        public AmmoCaliberType SupportedCaliber => _supportedCaliber;
        public uint Capacity => _capacity;
        public uint Remaining => _remaining;
        public AmmoData LoadedAmmoData => _loadedAmmoData;

        public GunAttachmentRuntimeData_Magazine(GunAttachmentData_Magazine baseData, int initialCount = 1) : base(baseData, initialCount)
        {
            _supportedCaliber = baseData.CompatibleCaliber;
            _capacity = baseData.Capacity;
            _remaining = (uint)Mathf.Min(baseData.DefaultAmmoNum, _capacity);
        }

        /// <summary>
        /// UI等からAmmoを装填する専用メソッド
        /// </summary>
        /// <param name="ammoStack">装填するAmmoRuntimeData</param>
        /// <returns>完全に装填できた場合true、余りがある場合false</returns>
        public bool TryLoadFromAmmo(IItemRuntimeData ammoStack)
        {
            if (ammoStack == null || ammoStack.BaseData.ItemType != ItemType.Ammo)
                return false;

            AmmoData ammoData = ammoStack.BaseData as AmmoData;
            if (ammoData == null || ammoData.Caliber != _supportedCaliber)
                return false;

            _loadedAmmoData = ammoData;

            // 装填可能な数を計算
            uint space = _capacity - _remaining;//リロードできる「空き」
            int toLoad = (int)Mathf.Min(space, ammoStack.StackCount);

            _remaining += (uint)toLoad;
            ammoStack.ReduceStack(toLoad);

            // 完全に装填できたか
            return ammoStack.StackCount <= 0;
        }
        
        public bool ConsumeBullet()
        {
            if (_remaining > 0)
            {
                _remaining--;
                return true;
            }

            _loadedAmmoData = null;
            return false;
        }
    }
}
