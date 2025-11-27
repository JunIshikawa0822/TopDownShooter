using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ASystem
{
    protected GameStatus gameStat;
    protected GameEventBus gameEvent;
    public void Init(GameStatus gameStat, GameEventBus gameEvent)
    {
        this.gameStat = gameStat;
        this.gameEvent = gameEvent;
    }
    public abstract void OnSetUp();
}
