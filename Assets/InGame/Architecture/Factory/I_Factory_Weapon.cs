using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFactory_Weapon
{
    public IWeapon CreateConcreteWeapon();
}
