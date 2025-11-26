using System.Collections.Generic;
using Game.Data;
using UnityEngine;
using UnityEngine.XR;

public class WeaponSystem : ASystem, IOnUpdate
{
    //private List<IOnUpdate> _updatableList = new();
    private IObjectPool<Bullet> _bulletPool;
    private GunService _gunService;
    private BulletService _bulletService;

    private Dictionary<WeaponType, IObjectPool<AWeaponBase>> _weaponFactories;

    public bool IsActiveForUpdate => true;

    public override void OnSetUp()
    {
        _bulletPool = new ObjectPool<Bullet>(gameStat.bulletPoolTrans, new Factory_Bullet(gameStat.bulletPrefab));
        _bulletPool.PoolSetUp(20);

        _weaponFactories = new()
        {
            {WeaponType.Handgun, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_Handgun(gameStat.handgunPrefab/*, _bulletPool*/), "Handgun")},
            {WeaponType.AssultRifle, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_AssultRifle(gameStat.assultRiflePrefab/*, _bulletPool*/), "AssultRifle")},
            {WeaponType.SniperRifle, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_SniperRifle(gameStat.sniperRiflePrefab/*, _bulletPool*/), "SnipeRifle")},
            {WeaponType.SubMachineGun, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_SubMachinegun(gameStat.subMachinegunPrefab/*, _bulletPool*/), "SubMachinegun")},
            {WeaponType.Shotgun, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_Shotgun(gameStat.shotgunPrefab/*, _bulletPool*/), "Shotgun")}
        };

        foreach(KeyValuePair<WeaponType, IObjectPool<AWeaponBase>> set in _weaponFactories)
        {
            set.Value.PoolSetUp(2);
        }

        _bulletService = new(_bulletPool);
        _gunService = new(_bulletService);

        //こっからテスト用コード
        WeaponTestDataSet();
        gameStat.playerEquipWeapon = CreateWeapon(gameStat.playerWeaponRuntimeData);
    }

    public void OnUpdate()
    {
        _gunService.OnUpdate();
        _bulletService.OnUpdate();
    }

    public void Equip(IGun<GunRuntimeData> gun)
    {
        //_gunService.EquipGun(gun);
    }

    public void UnEquip(IGun<GunRuntimeData> gun)
    {
        //_gunService.UnequipGun(gun);
    }

    public void AttackStart()
    {
        
    }

    public void AttackProcess()
    {
        
    }

    public void AttackEnd()
    {
        
    }

    public AWeaponBase CreateWeapon(AWeaponRuntimeDataBase weaponRuntimeData)
    {
        if(weaponRuntimeData == null)return null;

        AWeaponBase weapon = null;

        weapon = _weaponFactories[weaponRuntimeData.WeaponBaseData.WeaponType].GetFromPool();

        if(weapon == null) return null;

        if(weapon is IGun<GunRuntimeData> gun)
        {
            gun.Initialize(weaponRuntimeData as GunRuntimeData);
            Equip(gun);
        } 

        //見た目オブジェクトをセットする処理
        GameObject weaponVisualPrefab = GameObject.Instantiate(weaponRuntimeData.BaseData.VisualData.Prefab);
        weapon.VisualSet(weaponVisualPrefab);
        //Attachmentの位置を示す空オブジェクトの場所を、それぞれデータに合わせて設定し直す処理

        return weapon;
    }

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
            Debug.Log("銃ではある");
            if(gameStat.playerWeaponData is GunData gunData)
            {
                // Debug.Log("GunTestDataSet");
                // Debug.Log("やあ" + gunData);
                gameStat.playerWeaponRuntimeData = new GunRuntimeData(gunData);
            }
        }
    }
}
