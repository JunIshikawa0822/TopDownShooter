namespace TopDownShooter.Core.Requests
{
    using System;
    using System.Collections.Generic;

    public sealed class RequestAggregator<T> : IRequestSink<T>, IRequestResult<T>, IRequestOwner<T>
    {
        private readonly IAggregationPolicy<T> _policy;
        private readonly T _defaultValue;
        private readonly List<RequestEntry<T>> _entries;
        private long _nextId;

        public RequestAggregator(IAggregationPolicy<T> policy, T defaultValue, int capacity)
        {
            _policy = policy;
            _defaultValue = defaultValue;
            _entries = new List<RequestEntry<T>>(capacity);
            Current = defaultValue;
        }

        public event Action<T> Changed;

        public T Current { get; private set; }

        public IReadOnlyList<RequestEntry<T>> ActiveRequests => _entries;

        public RequestHandle<T> Request(T value, int priority, string requester)
        {
            throw new NotImplementedException();
        }

        // 引換券からだけ呼ばれるよう、明示的な実装にして外には見せない
        void IRequestOwner<T>.Release(long id)
        {
            throw new NotImplementedException();
        }

        void IRequestOwner<T>.UpdateValue(long id, T value)
        {
            throw new NotImplementedException();
        }

        private void Recalculate()
        {
            throw new NotImplementedException();
        }
    }
}
