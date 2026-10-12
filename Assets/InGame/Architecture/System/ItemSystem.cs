using System.Collections.Generic;
using Game.UI;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSystem : ASystem
{
    private readonly Dictionary<int, Inventory> _lootableInventoryDic = new();
    public override void OnSetUp()
    {
        gameStat.itemService = new ItemService();
        gameEvents.interactLootableEvent += ProvideContainer;
    }

    private void ProvideContainer(int lootableID, LootableType lootableType)
    {
        //lootableIDが含まれていない場合（初めて開ける箱の場合）は中身を生成する
        if (!_lootableInventoryDic.TryGetValue(lootableID, out Inventory inventory))
        {
            //InventoryDataを用いてInventoryを生成
            //
        }

        gameEvents.lootInventoryOpenEvent?.Invoke(inventory);
    }

    public override void OnDispose()
    {
        gameEvents.interactLootableEvent -= ProvideContainer;
    }
}
