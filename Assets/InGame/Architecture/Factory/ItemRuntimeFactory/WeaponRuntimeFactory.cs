using Game.Data;
using System;
public class WeaponRuntimeFactory : IItemRuntimeFactory
{
    public IItemRuntime CreateNewItemRuntime(ItemData itemData, int initialStack, Guid? runtimeGuid = null)
    {
        if (!(itemData is WeaponData weaponData)) return null;
        if (weaponData.WeaponType == WeaponType.Melee && weaponData is MeleeData meleeData)
        {
            return new MeleeRuntime(meleeData, initialStack, runtimeGuid);
        }
        else if (weaponData is GunData gunData)
        {
            return new GunRuntime(gunData, initialStack, runtimeGuid);
        }
        return null;
    }
}
