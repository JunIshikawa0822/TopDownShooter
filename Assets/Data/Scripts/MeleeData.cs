using UnityEngine;

namespace Game.Items
{
    [CreateAssetMenu(menuName = "Game/Item/MeleeData")]
    public class MeleeData : WeaponData
    {
        [Header("近接武器設定")]
        [SerializeField] private float attackSpeed;   // 攻撃速度
        [SerializeField] private float attackRange;   // 攻撃範囲
        [SerializeField] private float staminaCost;   // スタミナ消費量
        [SerializeField] private bool canGuard;       // ガード可能かどうか

        [Header("装飾スロット（カスタマイズ用）")]
        [SerializeField] private int ornamentSlotCount;
        // モンハン的な装飾品スロットのイメージ

        public float AttackSpeed => attackSpeed;
        public float AttackRange => attackRange;
        public float StaminaCost => staminaCost;
        public bool CanGuard => canGuard;
        public int OrnamentSlotCount => ornamentSlotCount;
    }
}

