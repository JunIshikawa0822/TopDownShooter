using Game.UI;
using Unity.VisualScripting;
using UnityEngine;

public class InventorySystem : ASystem
{
    private Inventory _inventoryModel;
    private InventoryView _inventoryView;
    private InventoryController _inventoryController;
    public override void OnSetUp()
    {
        _inventoryController = new InventoryController();
        _inventoryModel = new Inventory();
        gameStat.inventoryModel = _inventoryModel;

        //インベントリのシーンをロード
        sceneLoadBus.RequestRegisterCallback(SceneType.TetrisInventory, LoadUIScene);
        sceneLoadBus.RequestLoadScene(SceneType.TetrisInventory, RequestLoadState.Load);

        //インベントリ表示/非表示メソッド登録
        gameEvents.inventoryActiveEvent += ToggleInventory;
    }

    private void LoadUIScene(ISceneEntryPoint entryPoint)
    {
        if(!entryPoint.TryGetDependency<InventoryView>(out InventoryView inventoryView))
        {
            Debug.LogWarning("依存がない");
            return;
        }

        _inventoryView = inventoryView;
        
        _inventoryController.InitializeView(_inventoryView);
        _inventoryController.InitializeModel(_inventoryModel);

        _inventoryController.CraetePlayerContainer();

        Debug.Log($"{_inventoryView} : inventory確保成功");
    }

    private void ToggleInventory()
    {
        if(gameStat.isInventoryOpen)_inventoryController.Open();
        else _inventoryController.Close();
    }

    public override void OnDispose()
    {
        gameEvents.inventoryActiveEvent -= ToggleInventory;
    }
}
