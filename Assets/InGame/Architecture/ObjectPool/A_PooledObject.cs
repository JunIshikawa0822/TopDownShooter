using System;
using UnityEngine;
public abstract class APooledObject : MonoBehaviour
{
    private Action<APooledObject> poolAction;

    public virtual void ReturnToPool()
    {
        if (poolAction == null) return;
        poolAction?.Invoke(this);
    }
    
    public virtual void SetPoolAction<T>(Action<T> action) where T : APooledObject
    {
        // 登録されたAction<T>を、APooledObjectが要求するAction<APooledObject>に変換する
        poolAction = (APooledObject obj) => action((T)obj); // 内部キャスト
    }

    public void SetPoolAction(Action<APooledObject> action)
    {
        poolAction = action;
    }
}
