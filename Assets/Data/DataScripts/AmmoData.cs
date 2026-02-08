using UnityEngine;
using System.Collections.Generic;

namespace Game.Data
{
    [CreateAssetMenu(menuName = "MyGame/Ammo Data", fileName = "NewAmmoData")]
    public class AmmoData : ItemData
    {
        [Header("弾薬分類")]
        [SerializeField] private AmmoType _ammoType;

        [Header("基本パラメータ")]
        [SerializeField] private float _damage;
        [SerializeField] private float _penetrationPower;

        [Header("挙動カスタマイズ")]//いらないかも

        [SerializeField] private Color _tracerColor = Color.white;//弾道の色（属性で変えると一目でわかる

        [Header("特殊効果")]
        [Tooltip("物理弾の場合は空にしてください")]
        [SerializeField] private List<Effect> _effects = new List<Effect>();

        public AmmoType AmmoType => _ammoType;
        public float Damage => _damage;
        public float PenetrationPower => _penetrationPower;
        public Color TravelColor => _tracerColor;
        public List<Effect> Effects => _effects;
    }
}
