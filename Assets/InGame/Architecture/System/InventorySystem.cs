using Game.UI;
using Unity.VisualScripting;
using UnityEngine;

public class InventorySystem : ASystem
{
    private InventoryController _inventoryController;
    public override void OnSetUp()
    {
        _inventoryController = new InventoryController();
        gameStat.inventoryModel = new Inventory();

        //インベントリのシーンをロード
        sceneLoadBus.RequestRegisterCallback(SceneType.TetrisInventory, GetInventoryDependency);
        sceneLoadBus.RequestLoadScene(SceneType.TetrisInventory, RequestLoadState.Load);

        //インベントリ表示/非表示メソッド登録
        gameEvents.inventoryToggleEvent += ToggleInventory;
        gameEvents.lootContainerOpenEvent += OpenLootContainer;
    }

    private void GetInventoryDependency(ISceneEntryPoint entryPoint)
    {
        if(!entryPoint.TryGetDependency<InventoryView>(out InventoryView inventoryView))
        {
            Debug.LogWarning("依存がない");
            return;
        }

        gameStat.inventoryView = inventoryView;
        
        _inventoryController.InitializeView(inventoryView);
        _inventoryController.InitializeModel(gameStat.inventoryModel);

        _inventoryController.CraetePlayerContainer();

        Debug.Log($"{inventoryView} : inventory確保成功");
    }

    private void OpenLootContainer(Container container)
    {
        gameEvents.inventoryToggleEvent?.Invoke();
        //受け取ったContainerを元に、UIを開く処理
        Debug.Log("コンテナを開いた");
        gameStat.isInventoryOpen = true;
        ToggleInventory();
    }

    private void ToggleInventory()
    {
        if(gameStat.isInventoryOpen)
        {
            _inventoryController.Open();
            //インベントリ以外の（常駐でない）UIをとじる
            gameStat?.interactView.Hide();
        }
        else
        {
            _inventoryController.Close();
            //インベントリ以外の（常駐でない）UIを開く
            gameStat?.interactView.Show();
        }
    }

    public override void OnDispose()
    {
        gameEvents.inventoryToggleEvent -= ToggleInventory;
    }
}
