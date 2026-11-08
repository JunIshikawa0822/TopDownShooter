namespace TopDownShooter.Core.Requests
{
    using System;
    using System.Collections.Generic;

    public sealed class HighestPriorityPolicy<T> : IAggregationPolicy<T>
    {
        public T Aggregate(IReadOnlyList<RequestEntry<T>> entries, T defaultValue)
        {
            throw new NotImplementedException();
        }
    }
}
