using System;
using HTN;
using UnityEngine;
public abstract class AComparisonPreCondition : ATaskPreCondition
{
    protected enum ComparisonType
    {
        EqualTo,
        GreaterThan,
        LessThan,
        GreaterThanOrEqualTo,
        LessThanOrEqualTo,
        None
    }
    //[SerializeReference, SubclassSelector] private object left;
    [SerializeField] protected ComparisonType _comparisonType;
    //[SerializeReference, SubclassSelector] private object right;
}
