namespace TopDownShooter.Core.Timing
{
    using System;

    public interface ITimeSource
    {
        float Scale { get; }

        float DeltaTime { get; }

        // ゲームの時間の外の領域(Real・UI)では 0
        float FixedDeltaTime { get; }

        double ElapsedTime { get; }

        event Action<float> ScaleChanged;
    }
}
