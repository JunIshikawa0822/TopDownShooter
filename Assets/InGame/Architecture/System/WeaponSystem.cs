using System.Collections.Generic;
using Game.Data;
using UnityEngine;
using UnityEngine.XR;

public class WeaponSystem : ASystem, IOnUpdate, IOnFixedUpdate
{
    //private List<IOnUpdate> _updatableList = new();
    private IObjectPool<Bullet> _bulletPool;
    private IObjectPool<BulletVisual> _bulletVisualPool;
    private GunService _gunService;
    private BulletService _bulletService;
    private Dictionary<WeaponType, IObjectPool<AWeaponBase>> _weaponFactories;
    public bool IsActiveForUpdate => true;
    public bool IsActiveForFixedUpdate => true;
    public override void OnSetUp()
    {
        _bulletPool = new ObjectPool<Bullet>(gameStat.bulletPoolTrans, new Factory_Bullet(gameStat.bulletPrefab), "Bullet");
        _bulletVisualPool = new ObjectPool<BulletVisual>(gameStat.bulletVisualPoolTrans, new Factory_BulletVisual(gameStat.bulletVisualPrefab), "BulletVisual");
        
        _bulletPool.PoolSetUp(20);
        _bulletVisualPool.PoolSetUp(20);

        _weaponFactories = new()
        {
            {WeaponType.Handgun, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_Handgun(gameStat.handgunPrefab/*, _bulletPool*/), "Handgun")},
            {WeaponType.AssultRifle, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_AssultRifle(gameStat.assultRiflePrefab/*, _bulletPool*/), "AssultRifle")},
            {WeaponType.SniperRifle, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_SniperRifle(gameStat.sniperRiflePrefab/*, _bulletPool*/), "SnipeRifle")},
            {WeaponType.SubMachineGun, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_SubMachinegun(gameStat.subMachinegunPrefab/*, _bulletPool*/), "SubMachinegun")},
            {WeaponType.Shotgun, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_Shotgun(gameStat.shotgunPrefab/*, _bulletPool*/), "Shotgun")},

            {WeaponType.AssultRifle_Ray, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_AssultRifle_Ray(gameStat.assultRifleRayPrefab/*, _bulletPool*/), "AssultRifle_Ray")},
        };

        foreach(KeyValuePair<WeaponType, IObjectPool<AWeaponBase>> set in _weaponFactories)
        {
            set.Value.PoolSetUp(2);
        }

        _bulletService = new(_bulletPool, _bulletVisualPool);
        _gunService = new();

        //こっからテスト用コード
        WeaponTestDataSet();
        gameStat.playerEquipWeapon = CreateWeapon(gameStat.playerWeaponRuntimeData);

        gameEvents.attackStartEvent += AttackStart;
        gameEvents.attackEndEvent += AttackEnd;
    }

    public void OnUpdate()
    {
        _gunService.OnUpdate();
        _bulletService.OnUpdate();

        if(gameStat.isPressProcessing)
        {
            AttackProcess();
        }
    }

    public void OnFixedUpdate()
    {
        _bulletService.OnFixedUpdate();
    }

    public void AttackStart()
    {
        if(gameStat.playerEquipWeapon == null)return;
        gameStat.playerEquipWeapon.AttackStart();
        //Debug.Log("AttackStart");
    }

    public void AttackProcess()
    {
        if(gameStat.playerEquipWeapon == null)return;
        gameStat.playerEquipWeapon.AttackProcess();
        // Debug.Log("AttackProcess");
    }

    public void AttackEnd()
    {
        if(gameStat.playerEquipWeapon == null)return;
        gameStat.playerEquipWeapon.AttackEnd();
        //Debug.Log("AttackEnd");
    }

    private AWeaponBase CreateWeapon(AWeaponRuntimeDataBase weaponRuntimeData)
    {
        if(weaponRuntimeData == null)return null;
        AWeaponBase weapon = _weaponFactories[weaponRuntimeData.WeaponBaseData.WeaponType].GetFromPool();
        if(weapon == null) return null;

        //見た目オブジェクトをセットする処理
        GameObject weaponVisualPrefab = GameObject.Instantiate(weaponRuntimeData.BaseData.VisualData.Prefab);
        weapon.VisualSet(weaponVisualPrefab);

        if(weapon is IGun<GunRuntimeData> gun)
        {
            gun.Initialize(weaponRuntimeData as GunRuntimeData);
            gun.SetGunSurvice(_gunService);
            gun.SetBulletSurvice(_bulletService);
            _gunService.RegisterGun(gun);
        }
        // else if()
        // {
            
        // }

        return weapon;
    }

    //private IMelee<MeleeTuntimeData> CreateMelee(MeleeRuntimeData meleeRuntimeData)

    private void WeaponTestDataSet()
    {
        if(gameStat.playerWeaponData == null)return;

        if(gameStat.playerWeaponData.WeaponType == WeaponType.Melee)
        {
            if(gameStat.playerWeaponData is MeleeData meleeData)
            {
                gameStat.playerWeaponRuntimeData = new MeleeRuntimeData(meleeData);
                //Debug.Log("MeleeTestDataSet");
            }
        }
        else
        {
//            Debug.Log("銃ではある");
            if(gameStat.playerWeaponData is GunData gunData)
            {
                // Debug.Log("GunTestDataSet");
                // Debug.Log("やあ" + gunData);
                gameStat.playerWeaponRuntimeData = new GunRuntimeData(gunData);
            }
        }
    }
}
