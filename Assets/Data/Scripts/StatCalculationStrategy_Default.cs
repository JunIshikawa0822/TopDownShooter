using System.Collections.Generic;

public class StatCalculationStrategy_Default : IStatCalculationStrategy
{
    public float Calculate(float baseValue, IReadOnlyList<StatModifier> modifiers)
    {
        float addSum = 0;
        float mulSum = 0; // 倍率による「上乗せ分」を管理

        foreach (StatModifier mod in modifiers)
        {
            switch (mod.Type)
            {
                // 単純な加減算
                case StatModifierType.Add: 
                    addSum += mod.ModifyValue; break;
                    
                // 倍率補正（例：0.1なら10%増加）
                // Baseに対する「変化分」として蓄積する
                case StatModifierType.Multiply:
                    mulSum += baseValue * mod.ModifyValue; break;
            }
        }

        // 「加算の総計」と「倍率による変化の総計」の合算が「総変化量」
        return addSum + mulSum;
    }
}
