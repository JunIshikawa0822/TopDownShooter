using System;
using HTN;
using UnityEngine;
public abstract class AComparisonPreCondition : ATaskPreCondition
{
    private enum ComparisonType
    {
        EqualTo,
        GreaterThan,
        LessThan,
        GreaterThanOrEqualTo,
        LessThanOrEqualTo,
        None
    }
    //[SerializeReference, SubclassSelector] private object left;
    [SerializeField] private ComparisonType _comparisonType;
    //[SerializeReference, SubclassSelector] private object right;
}
