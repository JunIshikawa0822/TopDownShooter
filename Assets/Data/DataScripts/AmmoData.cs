using UnityEngine;

[CreateAssetMenu(menuName = "MyGame/Ammo Data", fileName = "NewAmmoData")]
public class AmmoData : ItemData
{
    [Header("弾薬情報")]
    [SerializeField] private AmmoCaliberType _caliberType;
    [SerializeField] private float _damage;
    [SerializeField] private float _penetrationPower;

    public AmmoCaliberType Caliber => _caliberType;
    public float Damage => _damage;
    public float PenetrationPower => _penetrationPower;
}
