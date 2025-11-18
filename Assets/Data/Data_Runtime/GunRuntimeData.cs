using System;
using System.Collections.Generic;
using System.Linq;

namespace Game.Items
{
    [Serializable]
    public class GunRuntimeData : AWeaponRuntimeDataBase
    {
        private readonly List<AttachmentSlotRuntimeData> _slots = new List<AttachmentSlotRuntimeData>();
        private readonly float[] _baseStats = new float[(int)GunStatType.Count];
        private readonly float[] _currentStats = new float[(int)GunStatType.Count];
        private FireMode _fireMode;//射撃モード
        private string _skinID;//将来的にスキン切りかえにつかうかも

        #region プロパティ
        public GunData GunBaseData => BaseData as GunData;
        public IReadOnlyList<AttachmentSlotRuntimeData> Slots => _slots;
        
        public float FireRate => _currentStats[(int)GunStatType.FireRate];
        public float Recoil => _currentStats[(int)GunStatType.Recoil];
        public float Accuracy => _currentStats[(int)GunStatType.Accuracy];
        public float BulletVelocity => _currentStats[(int)GunStatType.BulletVelocity];
        public FireMode CurrentFireMode => _fireMode;
        public string SkinID => _skinID;
        #endregion

        public GunRuntimeData(GunData baseData, int initialCount = 1) : base(baseData, initialCount)
        {
            _fireMode = baseData.FireMode;

            // // ---ベース値を配列に格納---
            _baseStats[(int)GunStatType.FireRate] = baseData.FireRate;
            _baseStats[(int)GunStatType.Recoil] = baseData.Recoil;
            _baseStats[(int)GunStatType.Accuracy] = baseData.Accuracy;
            _baseStats[(int)GunStatType.BulletVelocity] = baseData.BulletVelocity;

            // Array.Copy(_baseStats, _currentStats, _baseStats.Length);

            foreach (AttachmentSlotData slot in baseData.AbleAttachmentSlots)
            {
                AttachmentSlotRuntimeData runtimeSlot = new AttachmentSlotRuntimeData
                (
                    slot.SlotID,
                    slot.SlotType,
                    new AttachmentRuntimeData(slot.DefaultAttachmentData)
                );

                runtimeSlot.onChanged += RecalculateStats;
                _slots.Add(runtimeSlot);
            }
        }

        //補正値を反映した値にする計算
        public void RecalculateStats()
        {
            // ベース値をコピーして初期化
            Array.Copy(_baseStats, _currentStats, _baseStats.Length);

            float[] addBuffer = new float[_currentStats.Length];
            float[] mulBuffer = new float[_currentStats.Length];

            foreach (AttachmentSlotRuntimeData slot in _slots)
            {
                AttachmentRuntimeData attachment = slot.EquippedAttachment;
                if (attachment == null || attachment.Modifiers == null)
                    continue;

                foreach (GunStatModifier mod in attachment.Modifiers)
                {
                    int index = (int)mod.StatName;
                    mod.ApplyTo(ref addBuffer[index], ref mulBuffer[index]);
                }
            }

            for (int i = 0; i < _currentStats.Length; i++)
            {
                _currentStats[i] = (_baseStats[i] + addBuffer[i]) * (1 + mulBuffer[i]);
            }
        }

        //標準的なアタッチメント装着　スロットIDを指定して入れ替え
        public bool TryEquipAttachment(string slotID, AttachmentRuntimeData attachment)
        {
            if (attachment == null) return false;

            // 対応するスロットを探す
            AttachmentSlotRuntimeData slot = _slots.FirstOrDefault(s => s.SlotId == slotID);
            if (slot == null) return false;

            bool equipped = slot.Equip(attachment);
            return equipped;
        }

        //標準的なアタッチメント取り外し
        public AttachmentRuntimeData UnEquipAttachment(string slotID)
        {
            AttachmentSlotRuntimeData slot = FindSlotByID(slotID);
            if (slot == null) return null;

            AttachmentRuntimeData data = slot.UnEquip();
            return data;
        }

        public AttachmentRuntimeData Reload(AttachmentRuntimeData_Magazine newMagazine)
        {
            if (newMagazine == null || newMagazine.AttachmentData.attachmentType != AttachmentType.Magazine)
                return null;

            //マガジンスロットを取得
            AttachmentSlotRuntimeData slot = FindSlotByType(AttachmentType.Magazine);
            if (slot == null) return null;

            // 既存マガジンを退避
            AttachmentRuntimeData oldMagazine = slot.UnEquip();

            // 新しいマガジンを装着
            slot.Equip(newMagazine);
            return oldMagazine; // 交換前のマガジンを返す
        }

        private AttachmentSlotRuntimeData FindSlotByID(string slotID)
        {
            return _slots.FirstOrDefault(s => s.SlotId == slotID);
        }

        private AttachmentSlotRuntimeData FindSlotByType(AttachmentType type)
        {
            return _slots.FirstOrDefault(s => s.SlotType == type);
        }

        public AttachmentRuntimeData GetAttachmentByID(string slotID)
        {
            AttachmentSlotRuntimeData slot = FindSlotByID(slotID);
            if (slot == null) return null;

            AttachmentRuntimeData attachment = slot.EquippedAttachment;
            return attachment;
        }

        public AttachmentRuntimeData GetAttachmentByType(AttachmentType type)
        {
            AttachmentSlotRuntimeData slot = FindSlotByType(type);
            if (slot == null) return null;

            AttachmentRuntimeData attachment = slot.EquippedAttachment;
            return attachment;
        }
    }
}