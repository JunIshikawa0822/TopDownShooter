using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item Visual Data", fileName = "NewItemVisualData")]
public class ItemVisualData : ScriptableObject
{
    [Header("表示モデル")]
    [SerializeField] private GameObject _prefab;//実際の見た目
    [Header("インベントリ内のサイズ")]
    [SerializeField] private int width;
    [SerializeField] private int height;

    [Header("UI用アイコン")]
    [SerializeField] private Sprite _icon;

    [Header("サウンドなど")]
    [SerializeField] private AudioClip _pickupSound;
    [SerializeField] private AudioClip _useSound;

    public GameObject Prefab => _prefab;
    public Sprite Icon => _icon;
    public AudioClip PickupSound => _pickupSound;
    public AudioClip UseSound => _useSound;
}

[System.Serializable]
public class VisualVariant
{
    public string Id;                    // "Default", "Camo1", "Camo2" など
    public Material OverrideMaterial;    // 任意（nullなら無変更）
    public Texture OverrideTexture;      // 任意
}