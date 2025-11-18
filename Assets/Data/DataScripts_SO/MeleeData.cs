using UnityEngine;

namespace Game.Items
{
    [CreateAssetMenu(menuName = "Game/Weapon/MeleeData")]
    public class MeleeData : WeaponData
    {
        [Header("近接武器設定")]
        [SerializeField] private float _damage;
        [SerializeField] private float _attackSpeed;//攻撃速度
        [SerializeField] private float _attackRange;//攻撃範囲
        [SerializeField] private float _staminaCost;//スタミナ消費量
        [SerializeField] private bool _canGuard;//ガード可能かどうか

        [Header("装飾スロット（カスタマイズ用）")]
        [SerializeField] private int ornamentSlotCount;
        // モンハン的な装飾品スロットのイメージ

        public float AttackSpeed => _attackSpeed;
        public float AttackRange => _attackRange;
        public float StaminaCost => _staminaCost;
        public bool CanGuard => _canGuard;
        public int OrnamentSlotCount => ornamentSlotCount;
    }
}

