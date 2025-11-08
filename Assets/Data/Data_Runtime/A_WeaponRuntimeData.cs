using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game.Items
{
    public abstract class WeaponRuntimeData : ItemRuntimeDataBase
    {
        // WeaponData としての BaseData のラッパー（型安全に扱うため）
        public WeaponData WeaponBaseData => BaseData as WeaponData;
        public WeaponType WeaponType => WeaponBaseData.WeaponType;
        //つけられるエンチャント
        public IReadOnlyList<EnchantData> AttachableEnchants => WeaponBaseData.AttachableEnchants;
        //ついているエンチャント
        protected List<EnchantData> _attachedEnchats = new List<EnchantData>();

        protected WeaponRuntimeData(WeaponData baseData, int initialCount = 1) : base(baseData, initialCount)
        {

        }

        public void AddEnchant(EnchantData data)
        {
            //エンチャント付与のシステム

            //被りなしか判定するコードを書く
            //すでについているエンチャントを上書き（上位互換）するか判定するコードを書く
            //同時に併用できないエンチャントがないか判定するコードを書く

            _attachedEnchats.Add(data);
        }
    }
}