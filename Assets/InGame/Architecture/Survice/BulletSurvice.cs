using System.Collections.Generic;
public class BulletService : IOnUpdate
{
    private HashSet<Bullet> _activeBullets = new();
    private IObjectPool<Bullet> _bulletPool;
    public bool IsActiveForUpdate => true;

    public BulletService(IObjectPool<Bullet> bulletPool)
    {
        _bulletPool = bulletPool;
    }

    public void OnUpdate()
    {
        foreach(Bullet bullet in _activeBullets)
        {
            if(!bullet.IsActiveForUpdate)return;
            bullet.OnUpdate(); // 移動・寿命・当たり判定
        }
    }

    public Bullet GetBullet()
    {
        Bullet bullet = _bulletPool.GetFromPool();
        if (bullet == null) return null;

        _activeBullets.Add(bullet); // HashSet なので重複自動排除
        return bullet;
    }
}
