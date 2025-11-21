using System;
using UnityEngine;

//「どのステータスを」「どのように」「どれだけ」変更するかを1単位として表す構造体。
[Serializable]
public struct GunStatModifier
{
    [SerializeField] private GunStatType _statName;
    [SerializeField] private ModifierType _modifierType;
    [SerializeField] private float _modifyValue;
    public GunStatType StatName => _statName; // "Recoil", "Accuracy", "ReloadTime"など、補正する対象
    public ModifierType ModifierType => _modifierType; //加算/乗算
    public float ModifyValue => _modifyValue;
    
    public void ApplyTo(ref float addValue, ref float mulValue)
    {
        switch (_modifierType)
        {
            case ModifierType.Add:
                addValue += _modifyValue;
                break;
            case ModifierType.Multiply:
                mulValue += _modifyValue;
                break;
        }
    }
}

public enum ModifierType
{
    Add,
    Multiply
}