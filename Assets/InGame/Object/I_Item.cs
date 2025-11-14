using System.Collections;
using System.Collections.Generic;
using Game.Items;
using UnityEngine;

public interface IItem<out TRuntimeData> where TRuntimeData : AItemRuntimeDataBase
{
    TRuntimeData RuntimeData { get; }
}
