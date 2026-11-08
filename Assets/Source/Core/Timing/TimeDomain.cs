namespace TopDownShooter.Core.Timing
{
    using System;
    using TopDownShooter.Core.Requests;

    public sealed class TimeDomain : ITimeSource, IRequestSink<float>
    {
        private readonly TimeDomain _parent;
        private readonly RequestAggregator<float> _scaleRequests;

        public TimeDomain(string name, TimeDomain parent, int requestCapacity)
        {
            Name = name;
            _parent = parent;
            _scaleRequests = new RequestAggregator<float>(new MultiplyPolicy(), 1f, requestCapacity);
        }

        public event Action<float> ScaleChanged;

        public string Name { get; }

        public float Scale => (_parent != null ? _parent.Scale : 1f) * _scaleRequests.Current;

        public float DeltaTime { get; private set; }

        public float FixedDeltaTime { get; private set; }

        public double ElapsedTime { get; private set; }

        public RequestHandle<float> Request(float value, int priority, string requester)
        {
            return _scaleRequests.Request(value, priority, requester);
        }

        // 親から順に呼ばれる前提。親の倍率は、親の Advance が済んだ後の値を使う
        internal void Advance(float realDelta, float realFixedStep)
        {
            throw new NotImplementedException();
        }
    }
}
