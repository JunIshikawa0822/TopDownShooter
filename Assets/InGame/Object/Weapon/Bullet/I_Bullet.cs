using UnityEngine;

public interface IBullet
{
    public AmmoData Data{ get; }
    public AmmoCaliberType CaliberType { get; }
    public float PenetrationPower{ get; }
    public float Damage { get; }

    public void Init(AmmoData ammoData, Vector3 direction, float bulletSpeed);
}
