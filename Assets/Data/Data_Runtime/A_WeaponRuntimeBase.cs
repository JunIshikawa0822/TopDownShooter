using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Game.Data
{
    public abstract class AWeaponRuntimeBase : AItemRuntimeBase
    {
        //**********ベース値を管理する層**********
        protected Dictionary<string, float> _baseStats;
        //**********装備による一時的な値の変化量を管理する層**********
        private StatHandlerEquipment _statHandlerEquipment = new();
        private AttachmentSlot[] _attachmentSlots;//素の状態で存在するスロット
        //UIを表示する際、リストにはアタッチメントが並ぶ、
        //同種のアイテムスロットがある場合、位置が区別できない
        //TODO: いまのところは問題ないが、スロット位置が重要（3Dモデルへの反映）を行う場合は区別がいる
        private List<AttachmentSlot> _availableSlots = new();//拡張も含めたすべてのスロット
        //**********最終的な値を計算する層**********
        //private Dictionary<string, float> _finalizeStats = new();
        //private bool _isDirtyStats = true; //再計算必要か
        //**********プロパティ**********
        public WeaponData WeaponData => ItemData as WeaponData;
        public WeaponType WeaponType => WeaponData.WeaponType;
        public AWeaponRuntimeBase(WeaponData weaponData, int initialCount) : base(weaponData, initialCount)
        {
            //Data側で定義された基本スロット
            if (weaponData.EquippableTypes != null)
            {
                _attachmentSlots = new AttachmentSlot[weaponData.EquippableTypes.Length];
                for (int i = 0; i < _attachmentSlots.Length; ++i)
                {
                    _attachmentSlots[i] = new AttachmentSlot(weaponData.EquippableTypes[i]);
                }

                RecalculateSlots();
            }
        }

        #region 装備に関するメソッド

        /// <summary>
        /// スロットを指定してアタッチメントを装備する（UI側で選択されたSlotオブジェクトを直接渡す）
        /// Swapができるように返り値はAttachmentRuntime
        /// </summary>
        public virtual bool TryEquipAndSwapAttachment(AttachmentSlot slot, AttachmentRuntime attachment, out AttachmentRuntime swapAttachment)
        {
            swapAttachment = null;
            if (!slot.IsEquippable(attachment))
            {
                Debug.LogError("互換性のないスロットです。");
                return false;
            }

            AttachmentRuntime oldAttachment = slot.RemoveAttachment();
            _statHandlerEquipment.RemoveEquipmentProvider(oldAttachment);//Providerを外す

            slot.SetAttachment(attachment);
            _statHandlerEquipment.AddEquipmentProvider(attachment);//Providerを追加

            swapAttachment = oldAttachment;
            RecalculateSlots();//スロット再計算
            return true;
        }

        public virtual AttachmentRuntime UnEquipAttachment(AttachmentSlot slot)
        {
            // 実際に外れたものを取得
            AttachmentRuntime removedItem = slot.RemoveAttachment();

            if (removedItem != null)
            {
                //外れたアイテムの効果を消す
                _statHandlerEquipment.RemoveEquipmentProvider(removedItem);
            }

            RecalculateSlots();
            return removedItem;
        }

        //スロットの中身が変わったら呼び出す
        private void RecalculateSlots()
        {
            _availableSlots = GetAllAvailableSlots();
        }

        /// <summary>
        /// 武器本体および、装着中のアタッチメントから「現在利用可能なすべてのスロット」を再帰的に取得する
        /// </summary>
        private List<AttachmentSlot> GetAllAvailableSlots()
        {
            List<AttachmentSlot> allSlots = new List<AttachmentSlot>();

            //本体のスロットを追加
            foreach (AttachmentSlot slot in _attachmentSlots)
            {
                CollectSlotsRecursive(slot, allSlots);
            }

            return allSlots;
        }

        private void CollectSlotsRecursive(AttachmentSlot currentSlot, List<AttachmentSlot> resultList)
        {
            // まず自分自身を追加
            resultList.Add(currentSlot);

            // 何かが装着されていれば、そのアタッチメントが持つスロットも再帰的に追加
            if (currentSlot.Current != null)
            {
                foreach (AttachmentSlot subSlot in currentSlot.Current.SubSlots)
                {
                    CollectSlotsRecursive(subSlot, resultList);
                }
            }
        }

        //特定のスロットを探す
        protected List<AttachmentSlot> FindSlot(AttachmentType attachmentType)
        {
            return _availableSlots.Where(slot => slot.EquippableType == attachmentType).ToList();
        }

        #endregion

        #region 値に関するメソッド
        //基本的な加算
        protected virtual float GetEquipOffsetStat(string statName)
        {
            return _statHandlerEquipment.GetOffsetValue(statName, _baseStats[statName]);
        }
        #endregion

        #region エンチャントに関するメソッド
        //TODO: エンチャント保存のシステム
        public void AddEnchant(EnchantData data)
        {
            //被りなしか判定するコードを書く
            //すでについているエンチャントを上書き（上位互換）するか判定するコードを書く
            //同時に併用できないエンチャントがないか判定するコードを書く
        }
        #endregion
    }
}