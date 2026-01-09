using UnityEngine;
using Game.UI;

public class InventorySceneEntry : ASceneEntryPointBase
{
    [SerializeField] private InventoryView _inventoryView;
    [SerializeField] private InventoryEquipView _inventoryEquipView;
    protected override void BuildDependencies()
    {
        RegisterDependency<InventoryView>(_inventoryView);
        RegisterDependency<InventoryEquipView>(_inventoryEquipView);
    }

    //Awakeのタイミング
    protected override void OnSetUp()
    {
        base.OnSetUp();

        _inventoryView.Initialize();
        _inventoryEquipView.Initialize();
    }
}