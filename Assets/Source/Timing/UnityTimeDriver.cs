namespace TopDownShooter.Timing
{
    using System;
    using TopDownShooter.Core;

    // Unity の時間の仕組みには頼らず、Time.timeScale を 1 に固定し、物理もゲームの時間に合わせてここで進める
    public sealed class UnityTimeDriver : IInitializable, IOnPreUpdate
    {
        private readonly GameClocks _clocks;
        private readonly float _fixedStep;
        private readonly Action _onFixedStep;
        private float _accumulator;

        public UnityTimeDriver(GameClocks clocks, float fixedStep, Action onFixedStep)
        {
            _clocks = clocks;
            _fixedStep = fixedStep;
            _onFixedStep = onFixedStep;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void OnPreUpdate()
        {
            throw new NotImplementedException();
        }
    }
}
