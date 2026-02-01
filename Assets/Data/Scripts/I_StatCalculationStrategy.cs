using System.Collections.Generic;
//計算ロジックの戦略（Strategy Pattern）
public interface IStatCalculationStrategy
{
    //あくまで変化する差分を与える
    float Calculate(float baseValue, IReadOnlyList<StatModifier> modifiers);
}