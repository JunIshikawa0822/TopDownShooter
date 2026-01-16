using System.Collections.Generic;
using Game.UI;
using Unity.Mathematics;
using UnityEngine;

public class InteractSystem: ASystem, IOnLateUpdate
{
    private readonly Collider[] _results = new Collider[15];
    private LayerMask _interactableLayerMask;
    private const float _searchRadius = 3f;
    private const float _interactableRadius = 1.5f;
    private float _interactableDist;
    private IInteractable _currentInteractable;
    //private InteractController _interactController;
    public override void OnSetUp()
    {
        _interactableLayerMask = gameStat.interactableMask;
        //_interactController = new InteractController();

        //インタラクトの初期化を登録
        sceneLoadBus.RequestRegisterCallback(SceneType.InteractUI, GetInteractDependency);
        //これはUIのSystemでやった方がいいかも
        sceneLoadBus.RequestLoadScene(SceneType.InteractUI, RequestLoadState.Load);

        _interactableDist = _interactableRadius * _interactableRadius;

        gameEvents.interactEvent += OnInteractEvent;
    }

    public void OnLateUpdate()
    {
        if(gameStat.interactView == null)return;

        if(FindNearestInteractable(out IInteractable nearest))
        {
            _currentInteractable = nearest;
            gameStat.interactView.UpdateButtonPosition(_currentInteractable, gameStat.mainCamera);
            gameStat.interactView.DisplayButton(true);
        }
        else
        {
            _currentInteractable = null;
            gameStat.interactView.DisplayButton(false);
        }
    }

    private void OnInteractEvent()
    {
        if(_currentInteractable == null)return;

        switch(_currentInteractable.Type)
        {
            case InteractableType.Lootable : 
                if(_currentInteractable is ILootable lootable)
                    gameEvents.interactLootableEvent?.Invoke(lootable.LootableID, lootable.LootableType);
            break;
        }

        _currentInteractable.OnInteract();
    }

    //触れるオブジェクトを探す　ないならfalse　あるならtrueおよび当該オブジェクトを返す
    private bool FindNearestInteractable(out IInteractable nearest)
    {
        nearest = null;

        // 戻り値は「実際に見つかった個数」
        int count = Physics.OverlapSphereNonAlloc(
            gameStat.player.transform.position, 
            _searchRadius, 
            _results, 
            _interactableLayerMask
        );

        if(count <= 0)return false;

        float nearestDist = float.MaxValue;
        Collider bestCol = null;

        //見つかった要素分だけ回す
        for (int i = 0; i < count; ++i)
        {
            Collider col = _results[i];
            float dist = Vector3.SqrMagnitude(col.transform.position - gameStat.player.transform.position);

            //近い中でも、さらに規定の距離より近いかどうか（触れる距離にあるかどうか）
            if (dist < nearestDist && dist < _interactableDist)
            {
                nearestDist = dist;
                bestCol = col;
            }
        }

        if(bestCol == null)return false;
        if(bestCol.TryGetComponent<IInteractable>(out IInteractable interactable))
        {
            // Debug.Log("探索できるものを見つけた");
            nearest = interactable;
            return true;
            // _interactView.DisplayButton(true);
            // _interactView.UpdateButtonPosition(_currentInteractable, gameStat.mainCamera);
        }

        return false;
    }

    private void GetInteractDependency(ISceneEntryPoint entryPoint)
    {
        if(!entryPoint.TryGetDependency<InteractView>(out InteractView interactView))
        {
            Debug.LogWarning("依存がない");
            return;
        }

        gameStat.interactView = interactView;
        
        //_interactController.InitializeView(_interactView);

        // Debug.Log($"{interactView} : interactView確保成功");
    }

    public override void OnDispose()
    {
        gameEvents.interactEvent -= OnInteractEvent;
    }
}
