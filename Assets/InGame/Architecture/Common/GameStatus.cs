using Game.Data;
using UnityEngine;

[System.Serializable]
public class GameStatus
{
    [Header("Player")]
    public PlayerController player;

    [Header("Inputs")]
    public Vector2 moveDirection = Vector2.zero;
    public Vector2 screenPosition = Vector2.zero;
    public float maxVerticalAngle = 45f;
    [Header("ObjectPool")]
    public Transform bulletPoolTrans;
    public Transform gunPoolTrans;

    [Header("BulletPrefab")]
    public Bullet bulletPrefab;

    [Header("GunPrefab")]
    public Handgun handgunPrefab;
    public SubMachinegun subMachinegunPrefab;
    public AssultRifle assultRiflePrefab;
    public SniperRifle sniperRiflePrefab;
    public Shotgun shotgunPrefab;

    [Header("PlayerInventory")]
    //テスト用データ
    public WeaponData playerWeaponData;
    //テスト用ランタイムデータ
    [System.NonSerialized]public AWeaponRuntimeDataBase playerWeaponRuntimeData;
    //テスト用所持武器オブジェクト
    [System.NonSerialized]public AWeaponBase playerEquipWeapon;
}
