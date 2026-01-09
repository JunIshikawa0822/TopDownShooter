using Game.UI;
using UnityEngine;

public class InventorySystem : ASystem
{
    private InventoryView _inventoryView;
    public override void OnSetUp()
    {
        sceneLoadBus.RequestRegisterCallback(SceneType.TetrisInventory, LoadUIScene);
    }

    private void LoadUIScene(ISceneEntryPoint entryPoint)
    {
        if(!entryPoint.TryGetDependency<InventoryView>(out InventoryView inventoryView))
        {
            Debug.LogWarning("依存がない");
            return;
        }

        _inventoryView = inventoryView;
    }
}
