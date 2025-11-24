using Game.Data;
using UnityEngine;

[System.Serializable]
public class GameStatus
{
    [Header("Camera")]
    public Camera mainCamera;
    [Header("Player")]
    public PlayerController player;

    [Header("Inputs")]
    public Vector3 moveDirection = Vector3.zero;
    public Vector2 screenPosition = Vector2.zero;
    public Vector3 worldPosition = Vector3.zero;
    public GameObject cursorTrans;
    public float maxAimDistance = 50f;
    public float assistRadius = 0.5f; // エイムアシストの太さ
    public bool isCursorAssist = true;

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
