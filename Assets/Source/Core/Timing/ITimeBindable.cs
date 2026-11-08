namespace TopDownShooter.Core.Timing
{
    public interface ITimeBindable
    {
        void Bind(ITimeSource source);

        void Unbind();

        // 時間の場や凍結など、そのオブジェクトだけに掛かる倍率。反映される倍率はグループの倍率 × これ
        void SetLocalScale(float scale);
    }
}
