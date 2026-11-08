namespace TopDownShooter.Actions
{
    public readonly struct ActionHandle
    {
        public ActionHandle(int id, PlayerActionType actionType)
        {
            Id = id;
            ActionType = actionType;
        }

        public int Id { get; }

        public PlayerActionType ActionType { get; }
    }
}
