using Game.UI;
using System;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryController : AUIController
{
    //依存
    private Inventory _inventoryModel;
    private InventoryView _inventoryView;
    private InventoryEquipView _InventoryEquipView;

    //使うやつ
    private Guid _currentContainerGuid;
    private Guid _fromContainerGuid;
    private Guid _toContainerGuid;
    private Guid _currentItemGuid;
    public InventoryController(UIEvents uIEvents) : base(uIEvents){}

    public void Initialize(Inventory inventoryModel, InventoryView inventoryView)
    {
        _inventoryModel = inventoryModel;
        _inventoryView = inventoryView;
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    public void CreatePlayerContainer()
    {
        ContainerData containerData = Resources.Load<ContainerData>("Player_Test");

        Container container = new Container(containerData);
        
        _inventoryView.AddContainer(containerData.ContainerAsset, containerData.ContainerBuild, container.Guid);
        _inventoryModel.AddContainer(container);
    }
}
