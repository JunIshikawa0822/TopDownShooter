namespace TopDownShooter.Inputs
{
    using System;
    using System.Collections.Generic;

    public sealed class InputProcessor
    {
        private readonly Queue<InputRecord> _inputQueue;
        private readonly HashSet<InputActionType> _pressedInputs = new();

        public InputProcessor(int queueCapacity)
        {
            _inputQueue = new Queue<InputRecord>(queueCapacity);
        }

        public void RecordInput(InputRecord input)
        {
            throw new NotImplementedException();
        }

        public bool TryDequeueInput(out InputRecord input)
        {
            throw new NotImplementedException();
        }

        public bool IsPressed(InputActionType actionType)
        {
            throw new NotImplementedException();
        }
    }
}
