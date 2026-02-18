using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "MyGame/Item Visual Data", fileName = "NewItemVisualData")]
    public class ItemVisualData : ScriptableObject
    {
        [Header("表示モデル")]
        [SerializeField] private AssetReferenceGameObject _prefab;//実際のモデルの見た目
        [Header("インベントリ内のサイズ")]
        [SerializeField] private int _width;
        [SerializeField] private int _height;

        [Header("UI用アイコン")]
        [SerializeField] private AssetReferenceSprite _icon;

        [Header("サウンドなど")]
        [SerializeField] private AssetReferenceT<AudioClip> _pickupSound;
        [SerializeField] private AssetReferenceT<AudioClip> _useSound;

        public AssetReferenceGameObject Prefab => _prefab;
        public int Width => _width;
        public int Height => _height;
        public AssetReferenceSprite Icon => _icon;
        public AssetReferenceT<AudioClip> PickupSound => _pickupSound;
        public AssetReferenceT<AudioClip> UseSound => _useSound;
    }
}