using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOnFixedUpdate
{
    bool IsActiveForFixedUpdate{get;}
    public void OnFixedUpdate();
}
