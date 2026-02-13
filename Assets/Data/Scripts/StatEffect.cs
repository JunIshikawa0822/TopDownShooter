using System.Collections.Generic;
using System;
public class StatEffect : IStatModifierProvider
{
    private string _effectID;
    private readonly List<StatModifier> _modifiers;
    private float _duration;
    public string EffectID => _effectID;
    public float Duration => _duration;

    public StatEffect(string id, List<StatModifier> mods, float duration)
    {
        _effectID = id;
        _modifiers = mods;
        _duration = duration;
    }

    public bool CanRemove()
    {
        //時間による解除（無制限でなければ）
        bool timeOut = _duration <= 0;

        return timeOut;
    }

    public IEnumerable<StatModifier> GetModifiers() => _modifiers;

    public void Tick(float deltaTime)
    {
        _duration -= deltaTime;
    }
}