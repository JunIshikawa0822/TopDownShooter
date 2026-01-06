using System;
using System.Collections.Generic;
using Game.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class UIService
{
    private readonly VisualElement _root;
    private readonly Dictionary<Type, AUIController> _activeControllers = new();

    public UIService(VisualElement root)
    {
        _root = root;
    }

    public void Init()
    {
        _activeControllers.Clear();
        
        InventoryEquipView equipView = new(_root);

        //仮。ModelがUIServiceに作成されるのは変だと思うので。
        Inventory inventory = new();
        InventoryView inventoryView = new(_root);

        UIEvents uiEvents = new();
        InventoryController  inventoryController = new InventoryController(uiEvents);
        _activeControllers[typeof(InventoryController)] = inventoryController;
        
        inventoryController.Initialize(inventory, inventoryView);
        inventoryView.Initialize();
        equipView.Initialize();

        inventoryController.CreatePlayerContainer();
    }
}
