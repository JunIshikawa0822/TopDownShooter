using System;
using Game.Items;

//「どのステータスを」「どのように」「どれだけ」変更するかを1単位として表す構造体。
[Serializable]
public struct GunStatModifier
{
    public GunStatType statName; // "Recoil", "Accuracy", "ReloadTime"など、補正する対象
    public ModifierType type; //加算/乗算
    public float value;

    public void ApplyTo(ref float addValue, ref float mulValue)
    {
        switch (type)
        {
            case ModifierType.Add:
                addValue += value;
                break;
            case ModifierType.Multiply:
                mulValue += value;
                break;
        }
    }
}

public enum ModifierType
{
    Add,
    Multiply
}