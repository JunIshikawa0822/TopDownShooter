namespace TopDownShooter.Core.Requests
{
    using System;
    using System.Collections.Generic;

    public sealed class AnyTruePolicy : IAggregationPolicy<bool>
    {
        public bool Aggregate(IReadOnlyList<RequestEntry<bool>> entries, bool defaultValue)
        {
            throw new NotImplementedException();
        }
    }
}
