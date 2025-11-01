using UnityEngine;

[CreateAssetMenu(menuName = "Game/Enchant Data", fileName = "NewEnchant")]
public class EnchantData : ScriptableObject
{
    [Header("基本情報")]
    [SerializeField] private string _id;
    [SerializeField] private string _displayName;
    [TextArea] [SerializeField] private string _description;

    [Header("付与可能対象")]
    [SerializeField] private ItemType[] _applicableTypes; // 銃・近接など

    [Header("効果データ")]
    [SerializeField] private EnchantEffectType _effectType;
    [SerializeField] private float _magnitude;  // 効果量（例えばダメージ倍率など）
    [SerializeField] private float _duration;   // 持続時間があるタイプ用

    public string Id => _id;
    public string DisplayName => _displayName;
    public string Description => _description;
    public ItemType[] ApplicableTypes => _applicableTypes;
    public EnchantEffectType EffectType => _effectType;
    public float Magnitude => _magnitude;
    public float Duration => _duration;
}

