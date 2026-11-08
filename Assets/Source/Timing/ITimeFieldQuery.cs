namespace TopDownShooter.Timing
{
    using UnityEngine;

    public interface ITimeFieldQuery
    {
        float GetScaleAt(Vector3 position);
    }
}
