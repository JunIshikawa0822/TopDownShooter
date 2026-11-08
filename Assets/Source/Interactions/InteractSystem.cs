namespace TopDownShooter.Interactions
{
    using System;
    using TopDownShooter.Actions;
    using TopDownShooter.Core;
    using TopDownShooter.Inputs;

    public sealed class InteractSystem : IInputReceiver, IInitializable, IDisposable
    {
        private readonly IInputRouteRegistry _routeRegistry;
        private readonly ActionCoordinator _actionCoordinator;

        public InteractSystem(IInputRouteRegistry routeRegistry, ActionCoordinator actionCoordinator)
        {
            _routeRegistry = routeRegistry;
            _actionCoordinator = actionCoordinator;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void OnInputReceived(InputRecord input)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
