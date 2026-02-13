using UnityEngine;
using HTN;

[System.Serializable]
public class AFloatConstant : AFloatSource
{
    [SerializeField] private float value;
    public override float GetValue(WorldState worldState, SelfState selfState)
    {
        return value;
    }
}
