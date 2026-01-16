using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Data
{
    [Serializable]
    public class GunRuntimeData : AWeaponRuntimeDataBase
    {
        private readonly List<AttachmentSlotRuntimeData> _slots = new List<AttachmentSlotRuntimeData>();
        private readonly float[] _baseStats = new float[(int)GunStatType.Count];
        private readonly float[] _currentStats = new float[(int)GunStatType.Count];
        private FireType _fireType;//射撃モード
        private string _skinID;//将来的にスキン切りかえにつかうかも
        
        private AmmoData _currentLoadedAmmoData;
        //マガジンを使わないタイプだった場合、直で入ってる弾の数
        private int _internalAmmoRemaining;

        #region プロパティ
        public GunData GunBaseData => BaseData as GunData;
        public IReadOnlyList<AttachmentSlotRuntimeData> Slots => _slots;
        
        public float FireRate => _currentStats[(int)GunStatType.FireRate];
        public float HorizontalRecoil => _currentStats[(int)GunStatType.HorizontalRecoil];
        public float VerticalRecoil => _currentStats[(int)GunStatType.VerticalRecoil];
        public float Accuracy => _currentStats[(int)GunStatType.Accuracy];
        public float BulletVelocity => _currentStats[(int)GunStatType.BulletVelocity];
        public float MaxRange => _currentStats[(int)GunStatType.MaxRange];
        public FireType CurrentFireType => _fireType;
        public string SkinID => _skinID;

        public AmmoData CurrentAmmoData => _currentLoadedAmmoData;
        #endregion

        public GunRuntimeData(GunData baseData, int initialCount = 1) : base(baseData, initialCount)
        {
            // Debug.Log("GunRuntimeData constructing");
            _fireType = baseData.FireType;

            // // ---ベース値を配列に格納---
            _baseStats[(int)GunStatType.FireRate] = baseData.FireRate;
            _baseStats[(int)GunStatType.HorizontalRecoil] = baseData.HorizontalRecoil;
            _baseStats[(int)GunStatType.Accuracy] = baseData.Accuracy;
            _baseStats[(int)GunStatType.BulletVelocity] = baseData.BulletVelocity;
            _baseStats[(int)GunStatType.MaxRange] = baseData.MaxRange;

            Array.Copy(_baseStats, _currentStats, _baseStats.Length);

            foreach (AttachmentSlotData slot in baseData.AbleAttachmentSlots)
            {
                AttachmentRuntimeData defaultAttachment = null;

                if (slot.DefaultAttachmentData != null)
                {
                    //デフォルトアタッチメントのデータが存在する場合のみ、インスタンスを生成
                    defaultAttachment = new AttachmentRuntimeData(slot.DefaultAttachmentData);
                }

                AttachmentSlotRuntimeData runtimeSlot = new AttachmentSlotRuntimeData
                (
                    slot,
                    slot.SlotID,
                    slot.SlotType,
                    defaultAttachment//ここでnullが渡されるのはOK
                );

                runtimeSlot.onChanged += RecalculateStats;
                _slots.Add(runtimeSlot);
            }

            _internalAmmoRemaining = GunBaseData.InternalAmmoMax;
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
        public AttachmentRuntimeData UnEquipAttachmentByID(string slotID)
        {
            AttachmentSlotRuntimeData slot = FindSlotByID(slotID);
            if(slot.EquippedAttachment is AttachmentRuntimeData_Magazine)_currentLoadedAmmoData = null;
            if (slot == null) return null;

            AttachmentRuntimeData data = slot.UnEquip();
            return data;
        }

        public AttachmentRuntimeData UnEquipAttachmentByType(AttachmentType type)
        {
            AttachmentSlotRuntimeData slot = FindSlotByType(type);
            if(slot.EquippedAttachment is AttachmentRuntimeData_Magazine)_currentLoadedAmmoData = null;
            if (slot == null) return null;

            AttachmentRuntimeData data = slot.UnEquip();
            return data;
        }

        //マガジンを使う場合のリロード
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
            _currentLoadedAmmoData = newMagazine.LoadedAmmoData;
            return oldMagazine; // 交換前のマガジンを返す
        }

        //マガジンを使わない場合のリロード
        public int Reload(int loadAmmoNum, AmmoData loadAmmoData)
        {
            if(loadAmmoNum <= 0 || loadAmmoData == null) return 0;
            int loadCount = Mathf.Min(GunBaseData.InternalAmmoMax - _internalAmmoRemaining, loadAmmoNum);
            _internalAmmoRemaining += loadCount;
            _currentLoadedAmmoData = loadAmmoData;

            return loadAmmoNum - loadCount;
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

            return slot.EquippedAttachment;
        }

        public AttachmentRuntimeData GetAttachmentByType(AttachmentType type)
        {
            AttachmentSlotRuntimeData slot = FindSlotByType(type);
            if (slot == null) return null;

            return slot.EquippedAttachment;
        }

        public Vector3 GetAttachmentPos(AttachmentType type)
        {
            AttachmentSlotRuntimeData slot = FindSlotByType(type);
            if (slot == null) return Vector3.zero;

            return slot.BaseData.SlotPosition;
        }

        public Quaternion GetAttachmentRot(AttachmentType type)
        {
            AttachmentSlotRuntimeData slot = FindSlotByType(type);
            if (slot == null) return Quaternion.identity;

            return slot.BaseData.SlotRotation;
        }

        public bool TryConsumeRuntimeBullets()
        {
            //magazineがある？
            AttachmentSlotRuntimeData slotData = FindSlotByType(AttachmentType.Magazine);
            // if(_currentLoadedAmmoData == null)
            // {
            //     Debug.Log("AmmoDataがないよ");
            //     return false;
            // }

            //最初からmagazineがデータに設定されていない
            if(slotData == null)
            {
//                Debug.Log("マガジンがありません");
                if(GunBaseData.InternalAmmoMax <= 0)
                {
                    Debug.Log("Internal（マガジンを用いない弾数管理）も設定されていません");
                    return false;
                }
                
                if(_internalAmmoRemaining <= 0)
                {
                    Debug.Log("Internal（マガジンを用いない弾数管理）が設定されています");
                    Debug.Log("弾がありません");
                    _currentLoadedAmmoData = null;
                    return false;
                }

//                Debug.Log("Internal（マガジンを用いない弾数管理）が設定されています");
//                Debug.Log("弾を減らします");
                _internalAmmoRemaining--;
                return true;
            }

            Debug.Log("マガジンが設定されています");
            if(slotData.EquippedAttachment is AttachmentRuntimeData_Magazine magazine)
            {
                bool canConsume = magazine.ConsumeBullet();
                if(!canConsume)
                {
                    Debug.Log("弾がありません");
                    _currentLoadedAmmoData = null;
                }
                else
                {
                    Debug.Log("弾を減らします");
                }
                return canConsume;
            }

            Debug.Log("マガジンが入っていないか、型変換に問題があります");
            return false;
        }
    }
}