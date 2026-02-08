using Cysharp.Threading.Tasks;
using System.Threading;
using Game.Data;

public class Shotgun : AGunBase<GunRuntime>
{
    //TODO: 弾を拡散させる処理を追加
    //TODO: 複数の弾を発射する処理を追加
    public override void AttackStart()
    {
        if (GunRuntime.FireType == FireType.Burst) BurstFire().Forget();
        if (GunRuntime.FireType == FireType.FullAuto || GunRuntime.FireType == FireType.Semi)
        {
            if (!CanShoot()) return;

            _gunService.StartShooting(this);

            SetBullet(_muzzleTrans.forward);
            _gunService.RecordShotTime(this);
            InvokeMuzzleFlash().Forget();
        }
    }

    protected override async UniTaskVoid BurstFire()
    {
        CancellationToken ct = this.GetCancellationTokenOnDestroy();

        int count = GunRuntime.BurstCount;
        int intervalMs = (int)(GunRuntime.FireInterval * 1000);

        for (int i = 0; i < count; i++)
        {
            if (!CanShoot()) break;
            if (i == 0)
            {
                _gunService.StartShooting(this);
            }

            SetBullet(_muzzleTrans.forward);
            _gunService.RecordShotTime(this);
            InvokeMuzzleFlash().Forget();

            if (i < count - 1)
            {
                await UniTask.Delay(intervalMs, cancellationToken: ct);
            }
        }

        _gunService.StopShooting(this);
    }

    public override void AttackProcess()
    {
        if (GunRuntime.FireType == FireType.Semi || GunRuntime.FireType == FireType.Burst) return;
        if (GunRuntime.FireType == FireType.FullAuto)
        {
            if (!CanShoot()) return;

            SetBullet(_muzzleTrans.forward);
            _gunService.RecordShotTime(this);
            InvokeMuzzleFlash().Forget();
        }
    }
}
