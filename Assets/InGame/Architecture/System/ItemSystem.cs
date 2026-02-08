using System.Collections.Generic;
using Game.UI;
using Unity.VisualScripting;
using UnityEngine;

public class ItemSystem : ASystem
{
    private readonly Dictionary<int, Container> _lootableContainerDic = new();
    public override void OnSetUp()
    {
        gameStat.itemService = new ItemService();
        gameEvents.interactLootableEvent += ProvideContainer;
    }

    private void ProvideContainer(int lootableID, LootableType lootableType)
    {
        //lootableIDが含まれていない場合
        if (!_lootableContainerDic.TryGetValue(lootableID, out Container container))
        {
            //生成する
        }

        gameEvents.lootContainerOpenEvent?.Invoke(container);
    }

    public override void OnDispose()
    {
        gameEvents.interactLootableEvent -= ProvideContainer;
    }
}
