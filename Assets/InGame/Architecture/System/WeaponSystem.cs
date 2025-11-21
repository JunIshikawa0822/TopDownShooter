using System.Collections.Generic;
using Game.Data;
using UnityEngine;
using UnityEngine.XR;

public class WeaponSystem : ASystem, IOnUpdate
{
    private List<IOnUpdate> _updatableList = new();
    private IObjectPool<Bullet> _bulletPool;

    private Dictionary<WeaponType, IObjectPool<AWeaponBase>> _weaponFactories;

    public bool IsActiveForUpdate => true;

    public override void OnSetUp()
    {
        _bulletPool = new ObjectPool<Bullet>(gameStat.bulletPoolTrans, new Factory_Bullet(gameStat.bulletPrefab, UpdateRegistered));
        _bulletPool.PoolSetUp(20);

        _weaponFactories = new()
        {
            {WeaponType.Handgun, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_Handgun(gameStat.handgunPrefab, _bulletPool), "Handgun")},
            {WeaponType.AssultRifle, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_AssultRifle(gameStat.assultRiflePrefab, _bulletPool), "AssultRifle")},
            {WeaponType.SniperRifle, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_SniperRifle(gameStat.sniperRiflePrefab, _bulletPool), "SnipeRifle")},
            {WeaponType.SubMachineGun, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_SubMachinegun(gameStat.subMachinegunPrefab, _bulletPool), "SubMachinegun")},
            {WeaponType.Shotgun, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_Shotgun(gameStat.shotgunPrefab, _bulletPool), "Shotgun")}
        };

        foreach(KeyValuePair<WeaponType, IObjectPool<AWeaponBase>> set in _weaponFactories)
        {
            set.Value.PoolSetUp(2);
        }

        //こっからテスト用コード
        WeaponTestDataSet();
        WeaponSet(gameStat.playerWeaponRuntimeData);
    }

    public void OnUpdate()
    {
        for(int i = _updatableList.Count - 1; i >= 0; i--)
        {
            if(_updatableList[i].IsActiveForUpdate == false)return;

            _updatableList[i].OnUpdate();
        }
    }

    public void WeaponSet(AWeaponRuntimeDataBase weaponRuntimeData)
    {
        if(weaponRuntimeData == null)return;

        AWeaponBase weapon = null;

        weapon = _weaponFactories[weaponRuntimeData.WeaponBaseData.WeaponType].GetFromPool();

        if(weapon == null) return;

        if(weapon is AWeaponBase<GunRuntimeData> gun)
        {
            gun.Initialize(weaponRuntimeData as GunRuntimeData);
        } 

        //見た目オブジェクトをセットする処理
        GameObject weaponVisualPrefab = GameObject.Instantiate(weaponRuntimeData.BaseData.VisualData.Prefab);
        weapon.VisualSet(weaponVisualPrefab);

        gameStat.playerEquipWeapon = weapon;
        //Attachmentの位置を示す空オブジェクトの場所を、それぞれデータに合わせて設定し直す処理
    }

    private void UpdateRegistered(IOnUpdate updatable)
    {
        _updatableList.Add(updatable);
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
