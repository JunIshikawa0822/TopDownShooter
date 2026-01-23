using System.Collections.Generic;
using Game.Data;
using UnityEngine;
using System.Threading.Tasks;

public class WeaponSystem : ASystem, IOnUpdate, IOnFixedUpdate
{
    //private List<IOnUpdate> _updatableList = new();
    //private IObjectPool<Bullet> _bulletPool;
    private IObjectPool<BulletVisual> _bulletVisualPool;
    private GunService _gunService;
    private BulletService _bulletService;
    private Dictionary<WeaponType, IObjectPool<AWeaponBase>> _weaponFactories;
    private WeaponVisualLoader _weaponVisualLoader;
    public bool IsActiveForUpdate => true;
    public bool IsActiveForFixedUpdate => true;
    public override void OnSetUp()
    {
        _bulletVisualPool = new ObjectPool<BulletVisual>(gameStat.bulletVisualPoolTrans, new Factory_BulletVisual(gameStat.bulletVisualPrefab), "BulletVisual");
        _bulletVisualPool.PoolSetUp(20);

        _weaponFactories = new()
        {
            {WeaponType.Handgun, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_Handgun(gameStat.handgunPrefab, _gunService, _bulletService), "Handgun")},
            {WeaponType.AssultRifle, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_AssultRifle(gameStat.assultRiflePrefab, _gunService, _bulletService), "AssultRifle")},
            {WeaponType.SniperRifle, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_SniperRifle(gameStat.sniperRiflePrefab, _gunService, _bulletService), "SnipeRifle")},
            {WeaponType.SubMachinegun, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_SubMachinegun(gameStat.subMachinegunPrefab,_gunService, _bulletService), "SubMachinegun")},
            {WeaponType.Shotgun, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_Shotgun(gameStat.shotgunPrefab,_gunService, _bulletService), "Shotgun")},
        };

        _weaponVisualLoader = new();

        foreach(KeyValuePair<WeaponType, IObjectPool<AWeaponBase>> set in _weaponFactories)
        {
            set.Value.PoolSetUp(2);
        }

        _bulletService = new(_bulletVisualPool);
        _gunService = new();

        gameEvents.attackStartEvent += AttackStart;
        gameEvents.attackEndEvent += AttackEnd;

        WeaponTest();
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

    private void AttackStart()
    {
        if(gameStat.playerEquipWeapon == null)return;
        gameStat.playerEquipWeapon.AttackStart();
        //Debug.Log("AttackStart");
    }

    private void AttackProcess()
    {
        if(gameStat.playerEquipWeapon == null)return;
        gameStat.playerEquipWeapon.AttackProcess();
        // Debug.Log("AttackProcess");
    }

    private void AttackEnd()
    {
        if(gameStat.playerEquipWeapon == null)return;
        gameStat.playerEquipWeapon.AttackEnd();
        //Debug.Log("AttackEnd");
    }

    private async Task<AWeaponBase> CreateWeapon(AWeaponRuntimeDataBase weaponRuntimeData)
    {
        if(weaponRuntimeData == null)return null;
        AWeaponBase weapon = _weaponFactories[weaponRuntimeData.WeaponBaseData.WeaponType].GetFromPool();
        if(weapon == null) return null;

        GameObject weaponVisualInstance = await _weaponVisualLoader.LoadVisualAsync(weaponRuntimeData.VisualData.Prefab);
        weapon.VisualSet(weaponVisualInstance);
        weapon.Initialize(weaponRuntimeData);

        return weapon;
    }

    //private IMelee<MeleeTuntimeData> CreateMelee(MeleeRuntimeData meleeRuntimeData)

    private async void WeaponTest()
    {
        //こっからテスト用コード
        WeaponTestDataSet();
        gameStat.playerEquipWeapon = await CreateWeapon(gameStat.playerWeaponRuntimeData);
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
            // Debug.Log("銃ではある");
            if(gameStat.playerWeaponData is GunData gunData)
            {
                // Debug.Log("GunTestDataSet");
                // Debug.Log("やあ" + gunData);
                gameStat.playerWeaponRuntimeData = new GunRuntimeData(gunData);
            }
        }
    }

    public override void OnDispose()
    {
        gameEvents.attackStartEvent -= AttackStart;
        gameEvents.attackEndEvent -= AttackEnd;
    }
}
