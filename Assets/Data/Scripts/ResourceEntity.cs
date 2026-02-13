using UnityEngine;

public class ResourceEntity
{
    private float _current;
    public float Current => _current;

    public ResourceEntity(float initialValue)
    {
        _current = initialValue;
    }

    //数値が減ったり増えたりする場合
    public void Change(float amount, float max)
    {
        _current = Mathf.Clamp(_current + amount, 0, max);
    }

    //最大値が更新されたときに、現在の値が最大値を超えていたら最大値に合わせる
    public void SyncWithMax(float max)
    {
        if (_current > max) _current = max;
    }
}
