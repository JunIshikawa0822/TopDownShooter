using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ASystem
{
    protected GameStatus gameStat;
    public void Init(GameStatus gameStat)
    {
        this.gameStat = gameStat;
    }
    public abstract void OnSetUp();
}
