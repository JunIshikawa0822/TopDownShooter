using System;
using HTN;
using UnityEngine;

[System.Serializable]
public class PredicatePreCondition : ATaskPreCondition
{
    private enum PredicateType
    {
        And,
        Or
    }
    [SerializeReference, SubclassSelector] private ATaskPreCondition left;
    [SerializeField] private PredicateType _predicateType;
    [SerializeReference, SubclassSelector] private ATaskPreCondition right;
    public override bool CanExecute(WorldState state, SelfState selfState)
    {
        if (_predicateType == PredicateType.And)
        {
            return left.CanExecute(state, selfState) && right.CanExecute(state, selfState);
        }
        else
        {
            return left.CanExecute(state, selfState) || right.CanExecute(state, selfState);
        }
    }
}
