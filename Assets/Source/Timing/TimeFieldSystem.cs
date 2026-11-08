namespace TopDownShooter.Timing
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public sealed class TimeFieldSystem : ITimeFieldQuery, ITimeFieldRegistry
    {
        private readonly List<TimeField> _fields;

        public TimeFieldSystem(int capacity)
        {
            _fields = new List<TimeField>(capacity);
        }

        public void Register(TimeField field)
        {
            throw new NotImplementedException();
        }

        public void Unregister(TimeField field)
        {
            throw new NotImplementedException();
        }

        public float GetScaleAt(Vector3 position)
        {
            throw new NotImplementedException();
        }
    }
}
