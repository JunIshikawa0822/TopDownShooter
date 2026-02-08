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

        [Header("装備可能なアタッチメント")]
        [SerializeField] private AttachmentType[] _equippableTypes;

        [Header("エンチャント")]
        [SerializeField] private EnchantData[] _attachableEnchants;

        //[Header("Custom Effects")]
        //[SerializeField] private List<CustomEffect> _attachableEffects = new();

        public WeaponType WeaponType => _weaponType;
        public EnchantData[] AttachableEnchants => _attachableEnchants;
        public AnimatorOverrideController WeaponAnim => _weaponAnimation;
        public AttachmentType[] EquippableTypes => _equippableTypes;
    }
}

