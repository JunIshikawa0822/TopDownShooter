using System;

[Serializable]
public struct StatModifier
{
    public string statName; // "Recoil", "Accuracy", "ReloadTime"
    public ModifierType type; // Additive / Multiplicative
    public float value;
}

public enum ModifierType
{
    Add,
    Multiply
}
