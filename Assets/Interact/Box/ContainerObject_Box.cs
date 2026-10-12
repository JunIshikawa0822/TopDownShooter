using System.ComponentModel;
using UnityEngine;

public class ContainerObject_Box : MonoBehaviour, ILootable
{
    [SerializeField]private InteractableType _interactableType;
    [SerializeField]private LootableType _lootableType;
    [SerializeField]private int _id;
    public int LootableID => _id;
    public Vector3 WorldPosition => this.transform.position;
    public InteractableType Type => _interactableType;
    public LootableType LootableType => _lootableType;

    public void SetID(int id)
    {
        _id = id;
    }

    public void SetLootableType(LootableType lootableType)
    {
        _lootableType = lootableType;
    }

    public void OnInteract()
    {
        
    }
}
