using Game.Data;
using System;

public interface IItemRuntimeFactory
{
    public IItemRuntime CreateNewItemRuntime(ItemData data, int amount, Guid? guid = null);
}
