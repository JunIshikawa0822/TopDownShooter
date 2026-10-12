using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    InteractableType Type {get;}
    Vector3 WorldPosition { get; }

    /// プレイヤーが実際に操作したときに呼ばれる
    void OnInteract();
}
