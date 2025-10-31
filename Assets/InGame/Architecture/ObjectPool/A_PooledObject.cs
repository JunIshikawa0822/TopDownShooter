using System;
using UnityEngine;
public abstract class APooledObject : MonoBehaviour
{
    private Action<APooledObject> poolAction;

    public void ReturnToPool()
    {
        if(poolAction == null)return;
        poolAction?.Invoke(this);
    }

    public void SetPoolAction(Action<APooledObject> action)
    {
        poolAction += action;
    }
}
