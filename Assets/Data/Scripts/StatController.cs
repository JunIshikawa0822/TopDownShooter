using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
//Statusを管理する最上位
public class StatController
{
    //**********ベース値を管理する層**********
    private Dictionary<string, float> _baseStats = new ();

    //何らかの要因でBaseが成長したことを想定したメソッド
    public void SetBaseStat(Dictionary<string, float> baseStat)
    {
        _baseStats = baseStat;
    }
    
    //**********装備による一時的な値の変化量を管理する層**********

    StatHandlerEquipment _statHandlerEquipment = new();
    //**********バフによる一時的な値の変化量を管理する層**********
    StatHandlerEffect _statHandlerEffect = new();

    //**********最終的な値を計算する層**********
    private readonly Dictionary<string, float> _finalizeStats = new ();

    private void CalculateStat()
    {
        
    }
}