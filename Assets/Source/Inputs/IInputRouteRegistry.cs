namespace TopDownShooter.Inputs
{
    // 各 System には登録と解除だけを見せ、DispatchInputs などの配送の操作は見せない
    public interface IInputRouteRegistry
    {
        bool TryRegister(InputContext context, InputActionType actionType, IInputReceiver receiver);

        void Unregister(InputContext context, InputActionType actionType, IInputReceiver receiver);
    }
}
