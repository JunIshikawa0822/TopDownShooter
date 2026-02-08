using System.Collections.Generic;
using Game.Data;
using UnityEngine;
using Cysharp.Threading.Tasks;

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

        _bulletService = new(_bulletVisualPool);
        _gunService = new();
        _weaponVisualLoader = new();

        _weaponFactories = new()
        {
            {WeaponType.Handgun, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_Handgun(gameStat.handgunPrefab, _gunService, _bulletService), "Handgun")},
            {WeaponType.AssultRifle, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_AssultRifle(gameStat.assultRiflePrefab, _gunService, _bulletService), "AssultRifle")},
            {WeaponType.SniperRifle, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_SniperRifle(gameStat.sniperRiflePrefab, _gunService, _bulletService), "SnipeRifle")},
            {WeaponType.SubMachinegun, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_SubMachinegun(gameStat.subMachinegunPrefab,_gunService, _bulletService), "SubMachinegun")},
            {WeaponType.Shotgun, new ObjectPool<AWeaponBase>(gameStat.gunPoolTrans, new Factory_Shotgun(gameStat.shotgunPrefab,_gunService, _bulletService), "Shotgun")},
        };

        foreach(KeyValuePair<WeaponType, IObjectPool<AWeaponBase>> set in _weaponFactories)
        {
            set.Value.PoolSetUp(2);
        }

        gameEvents.attackStartEvent += AttackStart;
        gameEvents.attackEndEvent += AttackEnd;

        WeaponTest().Forget();
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

    //例えば「武器Aのロード中に、プレイヤーが急いで武器Bに切り替えた」場合、
    //TODO: 武器Aのロードを中断しないと、後から武器Aの見た目が届いて上書きされてしまうというバグの可能性有り
    private async UniTask<AWeaponBase> CreateWeapon(AWeaponRuntimeBase weaponRuntimeData)
    {
        if (weaponRuntimeData == null) return null;
        AWeaponBase weapon = _weaponFactories[weaponRuntimeData.WeaponData.WeaponType].GetFromPool();
        if (weapon == null) return null;

        try
        {
            //見た目のロード（ここで待機が発生）
            //キャンセルを考慮するなら .WithCancellation を推奨
            GameObject visualInstance = await _weaponVisualLoader.LoadVisualAsync(weaponRuntimeData.VisualData.Prefab);
            Debug.Log(visualInstance);
            //組み立て
            weapon.VisualSet(visualInstance);
            weapon.Initialize(weaponRuntimeData);

            return weapon;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"武器の生成に失敗しました: {e.Message}");
            
            //失敗した場合は、確保していた挙動オブジェクトをプールに返して掃除
            weapon.ReturnToPool();
            return null;
        }
    }

    //private IMelee<MeleeTuntimeData> CreateMelee(MeleeRuntimeData meleeRuntimeData)

    private async UniTaskVoid WeaponTest()
    {
        //こっからテスト用コード
        WeaponTestDataSet();
        gameStat.playerEquipWeapon = await CreateWeapon(gameStat.playerWeaponRuntimeData);
        gameStat.player.Equip(gameStat.playerEquipWeapon);
    }

    private void WeaponTestDataSet()
    {
        if(gameStat.playerWeaponData == null)return;

        if(gameStat.playerWeaponData.WeaponType == WeaponType.Melee)
        {
            if(gameStat.playerWeaponData is MeleeData meleeData)
            {
                gameStat.playerWeaponRuntimeData = new MeleeRuntime(meleeData);
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
                gameStat.playerWeaponRuntimeData = new GunRuntime(gunData);
            }
        }
    }

    public override void OnDispose()
    {
        gameEvents.attackStartEvent -= AttackStart;
        gameEvents.attackEndEvent -= AttackEnd;
    }
}
