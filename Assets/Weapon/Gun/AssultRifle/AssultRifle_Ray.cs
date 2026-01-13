using Game.Data;
using UnityEngine;
using System.Collections.Generic;

public class AssultRifle_Ray : AGunBase<GunRuntimeData>
{
    public override void AttackStart()
    {
        if(!_gunService.CanShoot(this))
        {
            Debug.Log("interval");
            return;
        }
        
        if(_gunService.IsBulletConsume())
        {
            if(!TryConsumeBullets())
            {
                Debug.Log("弾の消費に問題");
                return;
            }
        }

        if(!TryClipCheck())
        {
            Debug.Log("めりこみ");
            return;
        }
        
        Debug.Log("撃った");
        _gunService.StartShooting(this);
        
        SetRayBullet();
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

            SetRayBullet();
            _gunService.RecordShotTime(this);

            StartCoroutine(InvokeMuzzleFlash());
        }
    }
    public override void AttackEnd()
    {
        _gunService.StopShooting(this);
    }
}