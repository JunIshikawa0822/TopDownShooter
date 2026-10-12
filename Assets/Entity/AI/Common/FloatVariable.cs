using UnityEngine;
using HTN;

[System.Serializable]
public class FloatVariable : AFloatSource
{
    public string variableName;
    public override float GetValue(WorldState worldState, SelfState selfState)
    {
        return worldState.GetFloatVariable(variableName);
    }
}
