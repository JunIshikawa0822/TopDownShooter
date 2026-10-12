public interface IPoolManager//管理者の責務を反映
{
    void PoolSetUp(uint index);//プールを構築する
    void ReturnToPool(APooledObject pooledObject);//プールを清潔に保つ
}