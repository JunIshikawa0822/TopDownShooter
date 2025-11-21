using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game.Data
{
    public abstract class WeaponData : ItemData
    {
        [Header("武器固有情報")]
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private AnimatorOverrideController _weaponAnimation;
        //[SerializeField] private EquipmentSlot _equipSlot;

        [Header("エンチャント")]
        [SerializeField] private EnchantData[] _attachableEnchants;

        //[Header("Custom Effects")]
        //[SerializeField] private List<CustomEffect> _attachableEffects = new();

        // public float AttackSpeed => _attackSpeed;
        public WeaponType WeaponType => _weaponType;
        public IReadOnlyList<EnchantData> AttachableEnchants => _attachableEnchants;
        public AnimatorOverrideController WeaponAnim => _weaponAnimation;
        //public IReadOnlyList<CustomEffect> CustomEffects => customEffects;
    }
}

