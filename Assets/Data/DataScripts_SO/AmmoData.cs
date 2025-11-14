using UnityEngine;

[CreateAssetMenu(menuName = "Game/Item Data/Ammo", fileName = "NewAmmoData")]
public class AmmoData : ItemData
{
    [Header("弾薬情報")]
    [SerializeField] private AmmoCaliberType _caliber;
    [SerializeField] private float _damage;
    [SerializeField] private float _penetrationPower;
    [SerializeField] private float _maxLifeDistance;

    public AmmoCaliberType Caliber => _caliber;
    public float Damage => _damage;
    public float PenetrationPower => _penetrationPower;
    public float MaxLifeDistance => _maxLifeDistance;
}
