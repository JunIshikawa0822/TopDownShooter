namespace TopDownShooter.Core.Requests
{
    using System;
    using System.Collections.Generic;

    public sealed class MultiplyPolicy : IAggregationPolicy<float>
    {
        public float Aggregate(IReadOnlyList<RequestEntry<float>> entries, float defaultValue)
        {
            throw new NotImplementedException();
        }
    }
}
