using UnityEngine;

public class AttributeEntity
{
    private float _baseValue;
    private float _totalValue;
    private bool _isDirty = true;
    public float BaseValue => _baseValue;
    public float TotalValue => _isDirty ? RecalculateTotal() : _totalValue;

    public AttributeEntity(float baseValue)
    {
        _baseValue = baseValue;
        _totalValue = baseValue;
    }

    private float RecalculateTotal()
    {
        _totalValue = Mathf.Max(0, _baseValue);
        _isDirty = false;
        return _totalValue;
    }

    public void UpdateBase(float newBase)
    {
        _baseValue = newBase;
        _isDirty = true;
    }
}
