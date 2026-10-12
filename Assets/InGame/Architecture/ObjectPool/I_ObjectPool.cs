using UnityEngine;

public interface IObjectPool<T> where T : APooledObject 
{
    void PoolSetUp(uint index);
    T GetFromPool();
    void ReturnToPool(T pooledObject);
}
