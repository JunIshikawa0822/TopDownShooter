namespace TopDownShooter.Inputs
{
    public readonly struct InputRecord
    {
        public InputRecord(InputActionType actionType, InputEventType eventType)
        {
            ActionType = actionType;
            EventType = eventType;
        }

        public InputActionType ActionType { get; }

        public InputEventType EventType { get; }
    }
}
