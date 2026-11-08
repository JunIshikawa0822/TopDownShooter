namespace TopDownShooter.Inventories
{
    using System;
    using TopDownShooter.Core;
    using TopDownShooter.Inputs;

    public sealed class InventorySystem : IInputReceiver, IInitializable, IDisposable
    {
        private readonly IInputRouteRegistry _routeRegistry;

        public InventorySystem(IInputRouteRegistry routeRegistry)
        {
            _routeRegistry = routeRegistry;
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
