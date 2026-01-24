using UnityEngine;
public class WeaponAnchor : MonoBehaviour
{
    [SerializeField] private WeaponAnchorType _anchorType;
    public WeaponAnchorType AnchorType => _anchorType;
}