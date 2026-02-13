using UnityEngine;
using HTN;

[System.Serializable]
public class AFloatVariable : AFloatSource
{
    [StateVariable(TargetType = typeof(float))]
    public string variableName;
    public override float GetValue(WorldState worldState, SelfState selfState)
    {
        return worldState.GetFloatVariable(variableName);
    }
}
