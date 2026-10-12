using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDurabilityItem
{
    float Durability { get; }
    float MaxDurability { get; }
    void ApplyDurabilityChange(float delta);
}
