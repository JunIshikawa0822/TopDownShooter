using HTN;
using UnityEngine;

public class FloatComparisonPreCondition : AComparisonPreCondition
{
    [SerializeReference, SubclassSelector] private AFloatSource left;
    [SerializeReference, SubclassSelector] private AFloatSource right;

    public override bool CanExecute(WorldState worldState, SelfState selfState)
    {
        float leftValue = left.GetValue(worldState, selfState);
        float rightValue = right.GetValue(worldState, selfState);

        switch (_comparisonType)
        {
            case ComparisonType.EqualTo:
                return Mathf.Approximately(leftValue, rightValue);
            case ComparisonType.GreaterThan:
                return leftValue > rightValue;
            case ComparisonType.LessThan:
                return leftValue < rightValue;
            case ComparisonType.GreaterThanOrEqualTo:
                return leftValue >= rightValue;
            case ComparisonType.LessThanOrEqualTo:
                return leftValue <= rightValue;
            case ComparisonType.None:
            default:
                return false;
        }
    }
}
