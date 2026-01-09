using UnityEngine;
using System;

public class GameEventBus
{
    public Action attackStartEvent;
    public Action attackProcessEvent;
    public Action attackEndEvent;
    public Action<bool> inventoryActiveEvent; 
}
