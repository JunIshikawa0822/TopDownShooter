using System;
using UnityEngine;
public abstract class ItemData : ScriptableObject
{
    [Header("基本情報")]
    [SerializeField] private string _id;//ユニークID
    [SerializeField] private string _displayName;//表示名
    [SerializeField, TextArea] private string _description;//説明文

    [Header("基本パラメータ")]
    [SerializeField] private float _weight;//重量
    [SerializeField] private int _price;//売買価格
    [SerializeField] private int _stackableNumber;//スタックできる個数

    [Header("分類情報")]
    [SerializeField] private ItemType _itemType;//アイテム種別
    [SerializeField] private ItemTag _tags;//タグ（Flags)

    [Header("見た目情報")]
    [SerializeField] private ItemVisualData _visualData;//別SO参照

    // --- プロパティ ---
    public string ID => _id;
    public string DisplayName => _displayName;
    public string Description => _description;
    public float Weight => _weight;
    public int Price => _price;
    public int MaxStack => _stackableNumber;
    public ItemType ItemType => _itemType;
    public ItemTag Tags => _tags;
    public ItemVisualData VisualData => _visualData;
}


