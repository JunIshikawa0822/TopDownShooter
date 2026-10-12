using UnityEngine;
using Game.UI;

public class InventorySceneEntry : ASceneEntryPointBase
{
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private InventoryLootView _inventoryLootView;
    [SerializeField] private InventoryEquipView _inventoryEquipView;
    
    protected override void BuildDependencies()
    {
        RegisterDependency<InventoryView>(_inventoryView);
        RegisterDependency<InventoryLootView>(_inventoryLootView);
        RegisterDependency<InventoryEquipView>(_inventoryEquipView);
    }

    //Awakeのタイミング
    protected override void OnSetUp()
    {
        base.OnSetUp();

        _inventoryView.Initialize();
        _inventoryEquipView.Initialize();
        _inventoryLootView.Initialize();
    }
}