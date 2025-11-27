using Game.Data;
using UnityEngine;

public class AssultRifle : AGunBase<GunRuntimeData>
{
    public override void AttackStart()
    {
        if(!_gunService.CanShoot(this))
        {
            Debug.Log("interval");
            return;
        }
        
        if(!TryConsumeBullets())
        {
            Debug.Log("弾の消費に問題");
            return;
        }

        if(!TryClipCheck())
        {
            Debug.Log("めりこみ");
            return;
        }
        
        Debug.Log("撃った");
        _gunService.StartShooting(this);
        SpawnBullet();
        _gunService.RecordShotTime(this);

        StartCoroutine(InvokeMuzzleFlash());
        
    }
    public override void AttackProcess()
    {
        if(RuntimeData.CurrentFireType == FireType.Semi)return;

        if(RuntimeData.CurrentFireType == FireType.FullAuto)
        {
            if(!_gunService.CanShoot(this))return;
            
            if(!TryConsumeBullets())return;
            if(!TryClipCheck())return;

            Debug.Log("撃ってる");
            SpawnBullet();
            _gunService.RecordShotTime(this);

            StartCoroutine(InvokeMuzzleFlash());
        }
    }

    public override void AttackEnd()
    {
        _gunService.StopShooting(this);
    }
}