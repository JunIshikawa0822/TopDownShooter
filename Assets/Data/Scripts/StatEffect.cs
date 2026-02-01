using System.Collections.Generic;
using System;
public class StatusEffect : IStatModifierProvider
{
    private string _effectID;
    private readonly List<StatModifier> _modifiers;
    private float _duration;
    public string EffectID => _effectID;
    public float Duration => _duration;
    public Func<bool> RemovalCondition;

    public StatusEffect(string id, List<StatModifier> mods, float duration)
    {
        _effectID = id;
        _modifiers = mods;
        _duration = duration;
    }

    public bool CanRemove()
    {
        //時間による解除（無制限でなければ）
        bool timeOut = _duration <= 0;
        
        //特殊条件による解除（Delegateがあれば実行）
        bool condOut = RemovalCondition?.Invoke() ?? false;

        return timeOut || condOut;
    }

    public IEnumerable<StatModifier> GetModifiers() => _modifiers;

    public void Tick(float deltaTime)
    {
        _duration -= deltaTime;
    }
}