using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item Visual Data", fileName = "NewItemVisualData")]
public class ItemVisualData : ScriptableObject
{
    [Header("表示モデル")]
    [SerializeField] private GameObject _prefab;//実際の見た目
    [Header("インベントリ内のサイズ")]
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    [Header("UI用アイコン")]
    [SerializeField] private Sprite _icon;

    [Header("サウンドなど")]
    [SerializeField] private AudioClip _pickupSound;
    [SerializeField] private AudioClip _useSound;

    public GameObject Prefab => _prefab;
    public int Width => _width;
    public int Height => _height;
    public Sprite Icon => _icon;
    public AudioClip PickupSound => _pickupSound;
    public AudioClip UseSound => _useSound;
}