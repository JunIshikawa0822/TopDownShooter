using UnityEngine;

namespace Game.Data
{
    /// <summary>
    /// 抽象的なカスタム効果。ロジック層で評価される。
    /// </summary>
    public abstract class CustomEffect : ScriptableObject
    {
        [SerializeField] private string _effectName;
        [TextArea] [SerializeField] private string _description;

        public string EffectName => _effectName;
        public string Description => _description;

        /// <summary>
        /// 効果発動処理。ロジック層で呼ばれる。
        /// </summary>
        public abstract void ApplyEffect(GameObject target);

        /// <summary>
        /// 効果解除処理。装備解除などの際に呼ばれる。
        /// </summary>
        public virtual void RemoveEffect(GameObject target) { }
    }
}

