using System.Collections.Generic;
//補正値の供給源（アタッチメント、装備など）
public interface IStatModifierProvider
{
    IEnumerable<StatModifier> GetModifiers();
}