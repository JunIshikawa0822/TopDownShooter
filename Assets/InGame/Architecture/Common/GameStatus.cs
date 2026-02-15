using System;
using Game.Data;
using Game.UI;
using UnityEngine;

[System.Serializable]
public class GameStatus
{
    [SerializeReference]public AWeaponBase equippedWeapon;
    [Header("Camera")]
    public Camera mainCamera;
    [Header("Player")]
    public PlayerController player;

    [Header("Inputs")]
    [HideInInspector]
    public bool isAttackProcessing = false;
    public bool isAttackSupportInput = false;
    public Vector3 moveDirection = Vector3.zero;
    public bool isSprinting = false;
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
    public LayerMask interactableMask;

    [Header("ObjectPool")]
    public Transform bulletPoolTrans;
    public Transform bulletVisualPoolTrans;
    public Transform gunPoolTrans;

    [Header("BulletPrefab")]
    public BulletVisual bulletVisualPrefab;

    [Header("GunPrefab")]
    public Handgun handgunPrefab;
    public SubMachinegun subMachinegunPrefab;
    public AssultRifle assultRiflePrefab;
    public SniperRifle sniperRiflePrefab;
    public Shotgun shotgunPrefab;

    //public AssultRifle_Ray assultRifleRayPrefab;

    [Header("PlayerInventory")]
    //テスト用データ
    public WeaponData playerWeaponData;
    [HideInInspector] public bool isInventoryOpen = false;

    //テスト用ランタイムデータ
    [HideInInspector] public AWeaponRuntimeBase playerWeaponRuntimeData;
    //テスト用所持武器オブジェクト
    [HideInInspector] public AWeaponBase playerEquipWeapon;

    //シーンのロードに関する部分
    //インベントリモデル
    [HideInInspector] public Inventory inventoryModel;
    [HideInInspector] public InventoryView inventoryView;
    [HideInInspector] public InventoryEquipView inventoryEquipView;
    [HideInInspector] public InteractView interactView;

    [Header("Item")]
    [HideInInspector] public IItemService itemService;
}
