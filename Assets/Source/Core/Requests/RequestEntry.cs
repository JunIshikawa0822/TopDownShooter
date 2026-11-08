namespace TopDownShooter.Core.Requests
{
    public readonly struct RequestEntry<T>
    {
        public RequestEntry(T value, int priority, string requester, long id)
        {
            Value = value;
            Priority = priority;
            Requester = requester;
            Id = id;
        }

        public T Value { get; }

        public int Priority { get; }

        public string Requester { get; }

        public long Id { get; }
    }
}
