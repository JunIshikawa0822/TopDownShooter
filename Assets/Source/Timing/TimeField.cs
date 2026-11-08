namespace TopDownShooter.Timing
{
    using UnityEngine;

    // 置く側が起動時に作って持ち、使うたびに Register / Unregister する(ゲーム中に new しない)
    public sealed class TimeField
    {
        public Vector3 Center { get; set; }

        public float Radius { get; set; }

        public float Scale { get; set; }
    }
}
