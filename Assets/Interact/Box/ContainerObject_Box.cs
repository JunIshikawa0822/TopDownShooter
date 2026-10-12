using System.ComponentModel;
using UnityEngine;

public class ContainerObject_Box : MonoBehaviour, ILootable
{
    [SerializeField]private InteractableType _interactableType;
    [SerializeField]private LootableType _lootableType = LootableType.Crate;
    [SerializeField]private int _id;
    public int LootableID => _id;
    public Vector3 WorldPosition => this.transform.position;
    public InteractableType Type => _interactableType;
    public LootableType LootableType => _lootableType;

    //TODO: ステージ生成時にidをセットする処理を追加する
    //テストするときは便宜上のidをセットしておく
    public void SetID(int id)
    {
        _id = id;
    }

    //TODO: lootableTypeをセットする処理を追加する（デフォルトはCrate）
    public void SetLootableType(LootableType lootableType)
    {
        _lootableType = lootableType;
    }

    public void OnInteract()
    {
        
    }
}
