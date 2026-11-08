namespace TopDownShooter.Inputs
{
    using System;
    using System.Collections.Generic;
    using TopDownShooter.Core;
    using UnityEngine.InputSystem;

    public sealed class InputRecorder : IInitializable, IDisposable
    {
        private readonly InputProcessor _processor;
        private readonly Dictionary<InputAction, InputActionType> _actionTypes;

        public InputRecorder(InputProcessor processor, int actionCapacity)
        {
            _processor = processor;
            _actionTypes = new Dictionary<InputAction, InputActionType>(actionCapacity);
        }

        public void Bind(InputAction action, InputActionType actionType)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void OnActionStarted(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        private void OnActionPerformed(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        private void OnActionCanceled(InputAction.CallbackContext context)
        {
            throw new NotImplementedException();
        }

        private void RecordInput(InputActionType actionType, InputEventType eventType)
        {
            throw new NotImplementedException();
        }
    }
}
