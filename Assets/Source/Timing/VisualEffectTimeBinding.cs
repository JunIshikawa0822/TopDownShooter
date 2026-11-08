namespace TopDownShooter.Timing
{
    using System;
    using TopDownShooter.Core.Timing;
    using UnityEngine;
    using UnityEngine.VFX;

    public sealed class VisualEffectTimeBinding : MonoBehaviour, ITimeBindable
    {
        [SerializeField]
        private VisualEffect _effect;

        private ITimeSource _source;
        private float _localScale = 1f;

        public void Bind(ITimeSource source)
        {
            throw new NotImplementedException();
        }

        public void Unbind()
        {
            throw new NotImplementedException();
        }

        public void SetLocalScale(float scale)
        {
            throw new NotImplementedException();
        }

        private void Apply()
        {
            throw new NotImplementedException();
        }
    }
}
