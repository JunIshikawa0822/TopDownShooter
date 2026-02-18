using Game.UI;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Game.Data;

public class InventoryController : AUIController
{
    //依存
    private Inventory _inventoryModel;
    private InventoryView _inventoryView;
    private InventoryEquipView _InventoryEquipView;
    private InventoryLootView _inventoryLootView;

    //使うやつ
    private Guid _currentContainerGuid;
    private Guid _fromContainerGuid;
    private Guid _toContainerGuid;
    private Guid _currentItemGuid;

    public void InitializeModel(Inventory model)
    {
        _inventoryModel = model;
    }

    public void InitializeView(InventoryView view)
    {
        _inventoryView = view;
    }

    public void InitializeEquipView(InventoryEquipView view)
    {
        _InventoryEquipView = view;
    }

    public void InitializeLootView(InventoryLootView view)
    {
        _inventoryLootView = view;
    }

    public override void Open()
    {
        _inventoryView.Show();
        _InventoryEquipView.Show();
        _inventoryLootView.Show();
    }

    public override void Close()
    {
        _inventoryView.Hide();
        _InventoryEquipView.Hide();
        _inventoryLootView.Hide();
    }

    public void CreatePlayerContainer()
    {
        ContainerData playerContainerData = Resources.Load<ContainerData>("BackPack_1");
        Container container = new Container(playerContainerData);
        AddContainer(playerContainerData, container);
    }

    public void AddContainer(ContainerData containerData, Container container)
    {
        //TODO: 装備のたびにContainerTemplate生成は微妙　使い回し入れたい
        TemplateContainer containerTemplate = containerData.ContainerAsset.Instantiate();

        _inventoryView.AddContainer(containerTemplate, containerData.ContainerBuild, container.Guid);
        _inventoryModel.AddContainer(container);
    }
}
