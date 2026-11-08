namespace TopDownShooter.Inputs
{
    using System;

    // Dictionary のキーにするので IEquatable を実装する。実装しないと、引くたびにボックス化で GC の確保が起きる
    public readonly struct InputRouteKey : IEquatable<InputRouteKey>
    {
        public InputRouteKey(InputContext context, InputActionType actionType)
        {
            Context = context;
            ActionType = actionType;
        }

        public InputContext Context { get; }

        public InputActionType ActionType { get; }

        public bool Equals(InputRouteKey other)
        {
            return Context == other.Context && ActionType == other.ActionType;
        }

        public override bool Equals(object obj)
        {
            return obj is InputRouteKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return ((int)Context << 16) ^ (int)ActionType;
        }
    }
}
