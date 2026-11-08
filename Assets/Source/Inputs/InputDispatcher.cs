namespace TopDownShooter.Inputs
{
    using System;
    using System.Collections.Generic;
    using TopDownShooter.Core;
    using TopDownShooter.Core.Requests;

    public sealed class InputDispatcher : IInputRouteRegistry, IOnPreUpdate
    {
        private readonly InputProcessor _processor;
        private readonly Dictionary<InputRouteKey, IInputReceiver> _routes;

        // 押している途中でコンテキストが変わっても、Canceled を Started を受け取った System に届けるため
        private readonly Dictionary<InputActionType, IInputReceiver> _pressedReceivers;

        private readonly RequestAggregator<InputContext> _contextRequests;

        public InputDispatcher(InputProcessor processor, int routeCapacity, int contextRequestCapacity)
        {
            _processor = processor;
            _routes = new Dictionary<InputRouteKey, IInputReceiver>(routeCapacity);
            _pressedReceivers = new Dictionary<InputActionType, IInputReceiver>(routeCapacity);
            _contextRequests = new RequestAggregator<InputContext>(new HighestPriorityPolicy<InputContext>(), InputContext.Gameplay, contextRequestCapacity);
        }

        public IRequestSink<InputContext> ContextRequests => _contextRequests;

        public InputContext CurrentContext => _contextRequests.Current;

        public void OnPreUpdate()
        {
            DispatchInputs();
        }

        public void DispatchInputs()
        {
            throw new NotImplementedException();
        }

        public bool TryRegister(InputContext context, InputActionType actionType, IInputReceiver receiver)
        {
            throw new NotImplementedException();
        }

        public void Unregister(InputContext context, InputActionType actionType, IInputReceiver receiver)
        {
            throw new NotImplementedException();
        }

        private bool TryResolveReceiver(InputContext context, InputActionType actionType, out IInputReceiver receiver)
        {
            throw new NotImplementedException();
        }
    }
}
