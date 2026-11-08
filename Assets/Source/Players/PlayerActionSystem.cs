namespace TopDownShooter.Players
{
    using System;
    using System.Collections.Generic;
    using TopDownShooter.Actions;
    using TopDownShooter.Core;
    using TopDownShooter.Inputs;

    public sealed class PlayerActionSystem : IInputReceiver, IInitializable, IOnUpdate, IDisposable
    {
        private readonly IInputRouteRegistry _routeRegistry;
        private readonly ActionCoordinator _actionCoordinator;
        private readonly Queue<BufferedAction> _bufferedActions;

        public PlayerActionSystem(IInputRouteRegistry routeRegistry, ActionCoordinator actionCoordinator, int bufferCapacity)
        {
            _routeRegistry = routeRegistry;
            _actionCoordinator = actionCoordinator;
            _bufferedActions = new Queue<BufferedAction>(bufferCapacity);
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void OnInputReceived(InputRecord input)
        {
            throw new NotImplementedException();
        }

        public void OnUpdate()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void HandleAttackInput()
        {
            throw new NotImplementedException();
        }

        private void HandleDodgeInput()
        {
            throw new NotImplementedException();
        }

        private void HandleGuardInput()
        {
            throw new NotImplementedException();
        }

        private void TryExecuteAction(PlayerActionType actionType)
        {
            throw new NotImplementedException();
        }

        private void BufferAction(PlayerActionType actionType)
        {
            throw new NotImplementedException();
        }

        private void ProcessBufferedActions()
        {
            throw new NotImplementedException();
        }

        private void OnActionFinished(PlayerActionType actionType)
        {
            throw new NotImplementedException();
        }
    }
}
