[System.Serializable]
public struct StatModifier
{
    private string _statKey;
    private float _statValue;
    private StatModifierType _type;

    public string StatName => _statKey;
    public float ModifyValue => _statValue;
    public StatModifierType Type => _type;
}