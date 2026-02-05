using UnityEngine;
using Game.Data;
using System;
public class AttachmentRuntimeFactory : IItemRuntimeFactory
{
    public IItemRuntime CreateNewItemRuntime(ItemData itemData, int initialStack, Guid? runtimeGuid = null)
    {
        return null;
    }
}
