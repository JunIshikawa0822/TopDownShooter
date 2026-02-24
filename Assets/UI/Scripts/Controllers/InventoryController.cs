using Game.UI;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using Game.Data;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

public class InventoryController : AUIController
{
    //依存
    private Inventory _inventoryModel;

    private InventoryView _inventoryView;
    private InventoryEquipView _InventoryEquipView;
    private InventoryLootView _inventoryLootView;

    private VisualTreeAsset _itemTemplate;

    private AddressableProvider _itemSpriteLoader;

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

    //テスト用
    public void CreatePlayerContainer()
    {
        ContainerData playerContainerData = Resources.Load<ContainerData>("BackPack_1");
        EquipContainer(playerContainerData);
    }

    //InventoryViewにContainerの見た目を追加することと、InventoryLootViewにContainerの見た目を追加することは全く違う
    //・InventoryView：InventoryEquipの装備状況に応じて、ModelとViewに追加/削除する
    //・InventoryLootView：読み込んだInventoryLootの中身をViewに出すだけ
    //つまり追加するという結果は同じでもプロセスが全然違う

    //TODO: 本来は、InventoryEquipViewにContainerDataを持った装備が装備されるタイミングで、Containerを生成し、自身に装備する
    public void EquipContainer(ContainerData containerData)
    {
        Container container = new Container(containerData);
        //_inventoryViewにContainerの見た目を追加する
        TemplateContainer containerTemplate = containerData.ContainerAsset.Instantiate();
        _inventoryView.AddContainerComponent(containerTemplate, containerData.ContainerBuild, container.Guid);
        //_inventoryModelにContainerを追加する
        _inventoryModel.AddContainer(container);
    }

    public void LoadInventory()
    {
        LoadItem(_inventoryModel, _inventoryView).Forget();
    }

    public void LoadLootInventory(Inventory lootInventory)
    {
        foreach (Container container in lootInventory.Containers)
        {
            TemplateContainer containerTemplate = container.ContainerData.ContainerAsset.Instantiate();
            _inventoryLootView.AddContainerComponent(containerTemplate, container.ContainerData.ContainerBuild, container.Guid);
        }

        LoadItem(lootInventory, _inventoryLootView).Forget();
    }

    //とりあえず「読み込んだInventoryの中身をViewに出す」処理
    private async UniTaskVoid LoadItem(Inventory model, AContainerGridProvider view)
    {
        HashSet<InventoryItemData> processedItems = new();

        foreach (Container container in model.Containers)
            for (int i = 0; i < container.GridBlockCount; i++)
                foreach (InventoryItemData item in container.GridBlocks[i].Items)
                {
                    if (!processedItems.Add(item)) continue;
                    await LoadItemComponent(container.Guid, i, item, view);
                }
    }

    private async UniTask LoadItemComponent(Guid containerGuid, int gridIndex, InventoryItemData item, AContainerGridProvider view)
    {
        Sprite visualIcon = null;
        try
        {
            visualIcon = await _itemSpriteLoader.LoadAssetAsync<Sprite>(item.Runtime.VisualData.Icon);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"アイコンのロードに失敗しました: {e.Message}");
        }

        TemplateContainer itemTemplate = _itemTemplate.Instantiate();
        InventoryItemComponent itemComponent = view.CreateItemComponent(
            itemTemplate,
            item.ItemDataGuid,
            visualIcon,
            item.Width,
            item.Height
        );

        view.LoadToContainerComponent(containerGuid, gridIndex, item.OriginX, item.OriginY, item.DirectionDeg, itemComponent);
    }
}
