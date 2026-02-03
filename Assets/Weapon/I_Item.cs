using System.Collections;
using System.Collections.Generic;
using Game.Data;
using UnityEngine;

public interface IItem<out TRuntime> where TRuntime : AItemRuntimeBase
{
    TRuntime Runtime { get; }
}
