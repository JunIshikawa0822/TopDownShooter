using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnUpdate
{
    bool IsActiveForUpdate{get;}
    public void OnUpdate();
}
