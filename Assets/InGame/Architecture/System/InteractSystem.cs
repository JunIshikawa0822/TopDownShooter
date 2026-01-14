using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class InteractSystem: ASystem, IOnUpdate
{
    private readonly Collider[] _results = new Collider[15];
    private LayerMask _interactableLayerMask;
    private const float _interactRadius = 1f;
    public bool IsActiveForUpdate => true;
    public override void OnSetUp()
    {
        _interactableLayerMask = gameStat.interactableMask;
    }

    public void OnUpdate()
    {
        // 戻り値は「実際に見つかった個数」
        int count = Physics.OverlapSphereNonAlloc(
            gameStat.player.transform.position, 
            _interactRadius, 
            _results, 
            _interactableLayerMask
        );

        float nearest = float.MaxValue;
        Collider nearestCol = null;

        //見つかった要素分だけ回す
        for (int i = 0; i < count; ++i)
        {
            Collider col = _results[i];
            float dist = Vector3.SqrMagnitude(col.transform.position - gameStat.player.transform.position);

            if (dist < nearest) nearestCol = col;
        }

        if(nearestCol.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            
        }
    }
}
