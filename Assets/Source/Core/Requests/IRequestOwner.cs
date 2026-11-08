namespace TopDownShooter.Core.Requests
{
    // 引換券が本体の型を知らずに済むよう、取り下げと値の変更だけを公開する
    internal interface IRequestOwner<T>
    {
        void Release(long id);

        void UpdateValue(long id, T value);
    }
}
