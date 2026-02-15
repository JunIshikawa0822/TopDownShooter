using UnityEngine;
using System;

public class GameEventBus
{
    public Action<bool> attackStartEvent;
    //public Action<bool> attackProcessEvent;
    public Action<bool> attackEndEvent;
    public Action inventoryToggleEvent;
    public Action interactEvent;
    public Action<int, LootableType> interactLootableEvent;
    public Action<Container> lootContainerOpenEvent;
}
