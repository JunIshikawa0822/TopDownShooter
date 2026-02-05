using System;
using Game.Data;

public abstract class ArmorRuntimeFactory : IItemRuntimeFactory
{
    public IItemRuntime CreateNewItemRuntime(ItemData itemData, int initialStack, Guid? runtimeGuid = null)
    {
        return null;
    }
}
