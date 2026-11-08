namespace TopDownShooter.Timing
{
    using System;
    using TopDownShooter.Core.Timing;
    using UnityEngine;

    public sealed class AnimatorTimeBinding : MonoBehaviour, ITimeBindable
    {
        [SerializeField]
        private Animator _animator;

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
