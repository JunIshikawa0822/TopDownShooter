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
    public Vector3 worldPosition = Vector3.zero;
    public float maxVerticalAngle = 45f;
    public float maxAimDistance = 50f;
    public float assistRadius = 0.5f; // エイムアシストの太さ
    public bool isCursorAssist = true;

    //攻撃の水平位置
    //銃ならmuzzleがある場所
    //近接ならまた別
    public Transform baseTrans;

    [Header("LayerMask")]
    public LayerMask targetLayerMask;
    public LayerMask obstacleLayerMask;
    public LayerMask groundLayerMask;

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
