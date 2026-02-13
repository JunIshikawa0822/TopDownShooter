using UnityEngine;

namespace HTN
{
    public class SelfState
    {
        // 生存・リソース系
        public float CurrentHealth;
        public float MaxHealth;
        public int CurrentAmmo;
        public int MaxAmmo;
        public bool IsReloading;

        // 行動・能力系
        public bool IsStunned;
        public float MovementSpeed;

        // モチベーション・ターゲット系
        public bool HasTarget;
        public float DistanceToTarget;

        public SelfState()
        {
        }

        /// <summary>
        /// 自身を複製する
        /// </summary>
        /// <returns>自身の複製</returns>
        public SelfState CreateCopy()
        {
            SelfState copy = new SelfState();
            copy.CopyFrom(this);
            return copy;
        }

        public void CopyFrom(SelfState other)
        {
            this.CurrentHealth = other.CurrentHealth;
            this.MaxHealth = other.MaxHealth;
            this.CurrentAmmo = other.CurrentAmmo;
            this.MaxAmmo = other.MaxAmmo;
            this.IsReloading = other.IsReloading;
            this.IsStunned = other.IsStunned;
            this.MovementSpeed = other.MovementSpeed;
            this.HasTarget = other.HasTarget;
            this.DistanceToTarget = other.DistanceToTarget;
        }

        public float GetFloatVariable(string variableName)
        {
            // TODO: リフレクション等での取得を実装するか、
            // フィールドに直接アクセスする方式にする
            return 0;
        }
    }
}
