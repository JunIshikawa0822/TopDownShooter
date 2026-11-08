namespace TopDownShooter.Actions
{
    using System;
    using System.Collections.Generic;

    public sealed class ActionCoordinator
    {
        private readonly Dictionary<int, PlayerActionType> _activeActions;
        private readonly Dictionary<PlayerActionType, HashSet<PlayerActionType>> _conflictTable = new();
        private int _nextHandleId;

        public ActionCoordinator(int activeCapacity)
        {
            _activeActions = new Dictionary<int, PlayerActionType>(activeCapacity);
        }

        public void SetConflict(PlayerActionType running, PlayerActionType starting)
        {
            throw new NotImplementedException();
        }

        public ActionStartResult TryAcquire(PlayerActionType actionType, out ActionHandle handle)
        {
            throw new NotImplementedException();
        }

        public void Release(ActionHandle handle)
        {
            throw new NotImplementedException();
        }
    }
}
