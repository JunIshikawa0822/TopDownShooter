using System.Collections;
using System.Collections.Generic;
using Game.Items;
using UnityEngine;

public interface IWeaponHandler
{
    void Equip(IWeapon<AWeaponRuntimeDataBase> weapon);
}
