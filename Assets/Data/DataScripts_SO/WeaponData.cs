using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game.Items
{
    [CreateAssetMenu(menuName = "Game/Item/WeaponData")]
    public class WeaponData : ItemData
    {
        [SerializeField] private WeaponType _weaponType;
        //[SerializeField] private EquipmentSlot _equipSlot;

        [Header("Enchant")]
        [SerializeField] private List<EnchantData> _attachableEnchants;

        //[Header("Custom Effects")]
        //[SerializeField] private List<CustomEffect> _attachableEffects = new();

        // public float AttackSpeed => _attackSpeed;
        public WeaponType WeaponType => _weaponType;
        //public EquipmentSlot EquipSlot => _equipSlot;
        public IReadOnlyList<EnchantData> AttachableEnchants => _attachableEnchants;
        //public IReadOnlyList<CustomEffect> CustomEffects => customEffects;
    }
}

