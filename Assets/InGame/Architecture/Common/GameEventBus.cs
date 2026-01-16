using UnityEngine;
using System;

public class GameEventBus
{
    public Action attackStartEvent;
    public Action attackProcessEvent;
    public Action attackEndEvent;
    public Action inventoryToggleEvent;
    public Action interactEvent;
    public Action<int, LootableType> interactLootableEvent;
    public Action<Container> lootContainerOpenEvent;
}
