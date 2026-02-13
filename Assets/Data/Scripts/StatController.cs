using UnityEngine;
using System.Collections.Generic;

public class StatController : IOnUpdate
{
    // 大量のステータスを保持
    protected readonly Dictionary<string, AttributeEntity> _attributes = new();
    protected readonly Dictionary<string, ResourceEntity> _resources = new();
    protected StatHandlerEquipment _equipmentHandler = new();
    protected StatHandlerEffect _effectHandler = new();

    public bool IsActiveForUpdate => true;

    public virtual void Initialize()
    {
        //初期化の例
        _attributes["MaxHP"] = new AttributeEntity(100);
        _resources["CurrentHP"] = new ResourceEntity(100);
    }

    public virtual void OnUpdate()
    {
        _effectHandler.Tick(Time.deltaTime, _attributes, _resources);
    }

    public virtual void AddEquipmentProvider(IStatModifierProvider provider)
    {
        _equipmentHandler.AddEquipmentProvider(provider);
    }

    public virtual void RemoveEquipmentProvider(IStatModifierProvider provider)
    {
        _equipmentHandler.RemoveEquipmentProvider(provider);
    }

    public virtual void AddEffect(StatEffect effect)
    {
        _effectHandler.AddEffect(effect);
    }

    public void RemoveEffect(string effectKey)
    {
        _effectHandler.RemoveEffect(effectKey);
    }
}
