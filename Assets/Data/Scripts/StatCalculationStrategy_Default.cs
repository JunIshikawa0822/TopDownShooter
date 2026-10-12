using System.Collections.Generic;

public class StatCalculationStrategy_Default : IStatCalculationStrategy
{
    public float Calculate(float baseValue, IReadOnlyList<StatModifier> modifiers)
    {
        float addSum = 0;
        float mulPercentSum = 0;

        foreach (StatModifier mod in modifiers)
        {
            if (mod.Type == StatModifierType.Add) addSum += mod.ModifyValue;
            else if (mod.Type == StatModifierType.Multiply) mulPercentSum += mod.ModifyValue;
        }

        // (基礎値 + 装備加算) * (1 + 倍率合計) - 基礎値 = 変化分
        return (baseValue + addSum) * (1 + mulPercentSum) - baseValue;
    }
}
