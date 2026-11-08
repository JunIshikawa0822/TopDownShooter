namespace TopDownShooter.Core.Requests
{
    public interface IRequestSink<T>
    {
        RequestHandle<T> Request(T value, int priority, string requester);
    }
}
