using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic;
using System;

public class Inventory
{
    private readonly Dictionary<Guid, Container> _containersDic = new();
    public IReadOnlyCollection<Container> Containers => _containersDic.Values;

    public void AddContainer(Container container)
    {
        _containersDic[container.Guid] = container;
    }

    public void RemoveContainer(Guid containerGuid)
    {
        _containersDic.Remove(containerGuid);
    }
}