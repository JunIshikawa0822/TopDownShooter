using Game.Data;
using UnityEngine;

public class Handgun : AGunBase<GunRuntimeData>
{
    public override void AttackStart()
    {
        if(!_gunService.CanShoot(this))return;
        
        if(!TryConsumeBullets())return;
        if(!TryClipCheck())return;
        
        SpawnBullet();
        _gunService.RecordShotTime(this);
        
    }
    public override void AttackProcess()
    {
        if(RuntimeData.CurrentFireType == FireType.Semi)return;
        if(RuntimeData.CurrentFireType == FireType.FullAuto)
        {
            if(_gunService.CanShoot(this))return;
            
            if(!TryConsumeBullets())return;
            if(!TryClipCheck())return;

            SpawnBullet();
            _gunService.RecordShotTime(this);
            
        }
    }

    public override void AttackEnd()
    {
        
    }
}
