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
        //lootableIDが含まれていない場合（初めて開ける箱の場合）は中身を生成する
        if (!_lootableContainerDic.TryGetValue(lootableID, out Container container))
        {
            //ContainerDataを用いてContainerを生成
            //
        }

        gameEvents.lootContainerOpenEvent?.Invoke(container);
    }

    public override void OnDispose()
    {
        gameEvents.interactLootableEvent -= ProvideContainer;
    }
}
