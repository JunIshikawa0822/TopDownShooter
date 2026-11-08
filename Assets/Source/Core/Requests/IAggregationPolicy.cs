namespace TopDownShooter.Core.Requests
{
    using System.Collections.Generic;

    public interface IAggregationPolicy<T>
    {
        T Aggregate(IReadOnlyList<RequestEntry<T>> entries, T defaultValue);
    }
}
