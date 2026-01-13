using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ASystem
{
    protected GameStatus gameStat;
    protected GameEventBus gameEvents;
    protected SceneLoadBus sceneLoadBus;
    public void Init(GameStatus gameStat, GameEventBus gameEvent, SceneLoadBus sceneLoadBus)
    {
        this.gameStat = gameStat;
        this.gameEvents = gameEvent;
        this.sceneLoadBus = sceneLoadBus;
    }
    public abstract void OnSetUp();
    public virtual void OnDispose()
    {
        
    }
}
