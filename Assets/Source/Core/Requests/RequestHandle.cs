namespace TopDownShooter.Core.Requests
{
    using System;

    // 要求のたびに GC の確保を起こさないよう構造体にしている。番号は使い回さないので、
    // 古い引換券や2回目の Dispose は、本体で番号が見つからず何もしない
    public readonly struct RequestHandle<T> : IDisposable
    {
        private readonly IRequestOwner<T> _owner;
        private readonly long _id;

        internal RequestHandle(IRequestOwner<T> owner, long id)
        {
            _owner = owner;
            _id = id;
        }

        public void Update(T value)
        {
            _owner?.UpdateValue(_id, value);
        }

        public void Dispose()
        {
            _owner?.Release(_id);
        }
    }
}
