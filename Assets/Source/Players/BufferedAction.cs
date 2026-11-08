namespace TopDownShooter.Players
{
    using TopDownShooter.Actions;

    // InputRecord は「起きた入力の記録」、こちらは「まだ実行していない行動の要求」なので別の型にしている
    public readonly struct BufferedAction
    {
        public BufferedAction(PlayerActionType actionType, float expireTime)
        {
            ActionType = actionType;
            ExpireTime = expireTime;
        }

        public PlayerActionType ActionType { get; }

        public float ExpireTime { get; }
    }
}
