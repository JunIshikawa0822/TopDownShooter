using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AEffectServiceBase
{
    protected IObjectPool<Effect> _effectPool;

    protected AEffectServiceBase(IObjectPool<Effect> effect)
    {
        _effectPool = effect;
    }

    protected Effect SpawnEffect(Vector3 pos, Quaternion rot)
    {
        Effect effect = _effectPool.GetFromPool();
        return effect;
    }
}
