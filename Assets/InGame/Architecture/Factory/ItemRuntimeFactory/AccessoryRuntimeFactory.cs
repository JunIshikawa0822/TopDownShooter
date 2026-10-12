using System;
using Game.Data;

public class AccessoryRuntimeFactory : IItemRuntimeFactory
{
    public IItemRuntime CreateNewItemRuntime(ItemData itemData, int initialStack, Guid? runtimeGuid = null)
    {
        return null;
    }
}
