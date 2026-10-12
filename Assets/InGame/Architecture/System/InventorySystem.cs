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
        gameEvents.lootInventoryOpenEvent += OpenLootInventory;
    }

    private void GetInventoryDependency(ISceneEntryPoint entryPoint)
    {
        if (!entryPoint.TryGetDependency<InventoryView>(out InventoryView inventoryView))
        {
            Debug.LogWarning("InventoryViewへの依存がない");
            return;
        }

        if (!entryPoint.TryGetDependency<InventoryEquipView>(out InventoryEquipView inventoryEquipView))
        {
            Debug.LogWarning("InventoryEquipViewへの依存がない");
            return;
        }

        if (!entryPoint.TryGetDependency<InventoryLootView>(out InventoryLootView inventoryLootView))
        {
            Debug.LogWarning("InventoryLootViewへの依存がない");
            return;
        }

        gameStat.inventoryView = inventoryView;
        gameStat.inventoryEquipView = inventoryEquipView;
        gameStat.inventoryLootView = inventoryLootView;

        _inventoryController.InitializeView(inventoryView);
        _inventoryController.InitializeEquipView(inventoryEquipView);
        _inventoryController.InitializeLootView(inventoryLootView);

        _inventoryController.InitializeModel(gameStat.inventoryModel);

        _inventoryController.CreatePlayerContainer();

        Debug.Log($"{inventoryView} : inventory確保成功");
    }

    private void OpenLootInventory(Inventory inventory)
    {
        //受け取ったInventoryを元に、UIを開く処理
        Debug.Log("インベントリを開いた");
        gameStat.isInventoryOpen = true;
        gameStat.isLootOpen = true;
        gameEvents.inventoryToggleEvent?.Invoke();
    }

    private void ToggleInventory()
    {
        Debug.Log($"インベントリの開閉。現在の状態 : {gameStat.isInventoryOpen}");

        if (gameStat.isInventoryOpen)
        {
            _inventoryController.Open();

            //Lootしてない時はオフにする
            if (!gameStat.isLootOpen)
            {
                gameStat?.inventoryLootView.Hide();
            }

            //インベントリ以外の（常駐でない）UIをとじる
            gameStat?.interactView.Hide();
        }
        else
        {
            gameStat.isLootOpen = false;
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
