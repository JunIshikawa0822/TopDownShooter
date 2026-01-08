using UnityEngine;
using Game.UI;

public class InventorySceneEntry : ASceneEntryPointBase
{
    [SerializeField] private InventoryView _inventoryView;
    protected override void BuildDependencies()
    {
        RegisterDependency<InventoryView>(_inventoryView);
    }
}