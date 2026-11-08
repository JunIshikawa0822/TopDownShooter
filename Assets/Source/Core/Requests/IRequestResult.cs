namespace TopDownShooter.Core.Requests
{
    using System;

    public interface IRequestResult<T>
    {
        T Current { get; }

        event Action<T> Changed;
    }
}
