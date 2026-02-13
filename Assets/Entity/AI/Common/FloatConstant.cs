using UnityEngine;
using HTN;

[System.Serializable]
public class FloatConstant : AFloatSource
{
    [SerializeField] private float value;
    public override float GetValue(WorldState worldState, SelfState selfState)
    {
        return value;
    }
}
