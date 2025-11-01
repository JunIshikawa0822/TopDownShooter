using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game.Items
{
    [CreateAssetMenu(menuName = "Game/Item/WeaponData")]
    public class WeaponData : ItemData
    {
        [Header("Weapon Info")]
        [SerializeField] private float _baseDamage;
        // [SerializeField] private float _attackSpeed;
        [SerializeField] private float _durability;
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private EquipmentSlot _equipSlot;

        [Header("Enchant")]
        [SerializeField] private List<EnchantData> _attachableEnchant;

        [Header("Custom Effects")]
        //[SerializeField] private List<CustomEffect> _attachableEffects = new();

        // ==========================================================
        // プロパティ
        // ==========================================================
        public float BaseDamage => _baseDamage;
        // public float AttackSpeed => _attackSpeed;
        public float Durability => _durability;
        public WeaponType WeaponType => _weaponType;
        public EquipmentSlot EquipSlot => _equipSlot;
        public IReadOnlyList<EnchantData> Enchantments => _attachableEnchant;
        //public IReadOnlyList<CustomEffect> CustomEffects => customEffects;
    }
}

