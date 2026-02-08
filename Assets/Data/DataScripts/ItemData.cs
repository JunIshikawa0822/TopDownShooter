using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
namespace Game.Data
{
    public abstract class ItemData : ScriptableObject
    {
        [Header("基本情報")]
        [SerializeField] private string _id;//ユニークID
        [SerializeField] private string _displayName;//表示名
        [SerializeField, TextArea] private string _description;//説明文
        [SerializeField] private int _rank = 1; //ランク追加

        [Header("基本パラメータ")]
        [SerializeField] private float _weight;//重量
        [SerializeField] private int _basePrice;//売買価格
        [SerializeField] private int _stackableNumber;//スタックできる個数の上限

        [Header("分類情報")]
        [SerializeField] private ItemType _itemType;//アイテム種別
        [SerializeField] private ItemActionTag _actionTags;//どのように使用されるか
        [SerializeField] private ItemTraitTag _traitTags;//どのように振る舞うか

        [Header("見た目情報")]
        [SerializeField] private ItemVisualData _visualData;//別SO参照

        // --- プロパティ ---
        public string ItemID => _id;
        public string DisplayName => _displayName;
        public string Description => _description;
        public int Rank => _rank;
        public float Weight => _weight;
        public int Price => _basePrice;
        public int MaxStack => _stackableNumber;
        public ItemType ItemType => _itemType;
        public ItemActionTag ActionTags => _actionTags;
        public ItemTraitTag TraitTags => _traitTags;
        public ItemVisualData VisualData => _visualData;
    }
}



