using UnityEngine;
using System;
using System.Collections.Generic;

namespace Game.Items
{
    public abstract class WeaponData : ItemData
    {
        [Header("武器固有情報")]
        [SerializeField] private WeaponType _weaponType;
        [SerializeField] private GameObject _weaponPrefab;
        [SerializeField] private AnimatorOverrideController _weaponAnimation;
        //[SerializeField] private EquipmentSlot _equipSlot;

        [Header("エンチャント")]
        [SerializeField] private List<EnchantData> _attachableEnchants;

        //[Header("Custom Effects")]
        //[SerializeField] private List<CustomEffect> _attachableEffects = new();

        // public float AttackSpeed => _attackSpeed;
        public WeaponType WeaponType => _weaponType;
        public GameObject WeaponPrefab => _weaponPrefab;
        public IReadOnlyList<EnchantData> AttachableEnchants => _attachableEnchants;
        public AnimatorOverrideController WeaponAnim => _weaponAnimation;
        //public IReadOnlyList<CustomEffect> CustomEffects => customEffects;
    }
}

