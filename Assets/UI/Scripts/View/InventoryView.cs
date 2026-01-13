using System.Collections.Generic;
using Game.Data;
using Game.UI;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using System;

namespace Game.UI
{
    public class InventoryView : AUIView
    {
        private const int _cellSize = 48;
        private ScrollView _scrollViewParent;
        private VisualElement _scrollContentContainer;

        private VisualTreeAsset _cellTemplate;
        private VisualTreeAsset _containerTemplate;
        private VisualTreeAsset _itemTemplate;

        private Guid _selectingContainerGuid;
        private int _selectingGridBlockIndex = 0;
        
        private readonly Dictionary<Guid, ContainerComponent> _containerUIDic = new();

        protected override void SetVisualElements()
        {
            _scrollViewParent = _rootElement.Q<ScrollView>("container__scrollview");
            _scrollContentContainer = _scrollViewParent.Q<VisualElement>("unity-content-container");

            // //グリッドセルのロード
            // _cellTemplate = Resources.Load<VisualTreeAsset>("GridBlockCell");
            // //グリッドブロックのロード
            // _gridBlockTemplate = Resources.Load<VisualTreeAsset>("GridBlock");
            // //コンテナのロード
            // _containerTemplate = Resources.Load<VisualTreeAsset>("Container");
            // //アイテムUIのロード
            _itemTemplate = Resources.Load<VisualTreeAsset>("InventoryItem");
        }

        protected override void RegisterButtonCallbacks()
        {
            
        }

        // public void CreateDefaultContainer(int n)
        // {
        //     Debug.Log("実行");

        //     TemplateContainer containerUI = _containerTemplate.Instantiate();
        //     TemplateContainer gridBlockUI = _gridBlockTemplate.Instantiate();
        //     VisualElement gridBlockRoot = gridBlockUI.Q<VisualElement>("gridblock__root");

        //     for(int i = 0; i < n; ++i)
        //     {
        //         TemplateContainer cellUI = _cellTemplate.Instantiate();
        //         gridBlockRoot.Add(cellUI);
        //     }

        //     containerUI.Add(gridBlockRoot);
        //     _scrollContentContainer.Add(containerUI);
        //     //ContainerComponent defaultContainer = new(containerUI);
        // }
        private InventoryItemComponent CreateItemComponent(TemplateContainer itemTemplate, ItemVisualData visualData, Guid itemGuid)
        {
            InventoryItemComponent itemComponent = new InventoryItemComponent(itemTemplate, itemGuid);
            itemComponent.SetVisualData(visualData, _cellSize);
            //OnPointerEnterの登録
            //OnPointerLeaveの登録

            return itemComponent;
        }

        private GridCellComponent CreateGridCellComponent(VisualElement cellElement)
        {
            GridCellComponent cellComponent = new();
            cellComponent.SetVisualElements(cellElement);
            //OnPointerEnterの登録
            //OnPointerLeaveの登録
            cellComponent.RegisterPointerCallbacks();
            return cellComponent;
        }

        private GridBlockComponent CreateGridBlockComponent(VisualElement gridBlockElement, int gridBlockIndex, GridBlockData gridBlockData, int cellSize)
        {
            GridBlockComponent gridBlockComponent = new GridBlockComponent(gridBlockIndex, gridBlockData.width, gridBlockData.height, cellSize);
            gridBlockComponent.SetVisualElements(gridBlockElement);
            //OnPointerEnterの登録
            gridBlockComponent.OnPointerEnterEvent += SetSelectingGridBlockIndex;
            //OnPointerLeaveの登録
            gridBlockComponent.RegisterPointerCallbacks();
            return gridBlockComponent;
        }

        private ContainerComponent CreateContainerComponent(VisualElement containerElement, Guid containerGuid)
        {
            ContainerComponent containerComponent = new ContainerComponent(containerGuid);
            containerComponent.SetVisualElements(containerElement);
            //OnPointerEnterの登録
            containerComponent.OnPointerEnterEvent += SetSelectingContainerGuid;
            //OnPointerLeaveの登録
            containerComponent.RegisterPointerCallbacks();
            return containerComponent;
        }

        public void LoadItemToContainer(Guid containerGuid, int gridBlockIndex, int x, int y, int rotationDeg, InventoryItemComponent itemComponent)
        {
            _containerUIDic[containerGuid].PlaceItem(gridBlockIndex, x, y, rotationDeg, itemComponent);
        }

        public void AddContainer(VisualElement containerRoot, GridBlockData[] gridBlockDatas, Guid containerGuid)
        {
            // TemplateContainer containerRoot = containerAsset.Instantiate();
            List<VisualElement> gridBlockUIs = containerRoot.Query<VisualElement>(className: "GridBlock").ToList();

            if(gridBlockDatas.Length != gridBlockUIs.Count)
            {
                Debug.LogError("データとUIが異なります");
                return;
            }

            ContainerComponent container = CreateContainerComponent(containerRoot, containerGuid);

            for(int i = 0; i < gridBlockDatas.Length; ++i)
            {
                int count = gridBlockDatas[i].width * gridBlockDatas[i].height;
                List<VisualElement> cellUIs = gridBlockUIs[i].Query<VisualElement>(className: "GridCell").ToList();

                Debug.Log($"{gridBlockUIs.Count}");
                Debug.Log($"{cellUIs.Count}個のcell");
                if(count != cellUIs.Count)
                {
                    Debug.LogError($"{i}番めのgridblockでデータとUIが異なります");
                    return;
                }

                GridBlockComponent gridBlock = CreateGridBlockComponent(gridBlockUIs[i], i, gridBlockDatas[i], _cellSize);

                for(int j = 0; j < count; ++j)
                {
                    int x = count % gridBlockDatas[i].width;
                    int y = count / gridBlockDatas[i].height;
                    GridCellComponent cell = CreateGridCellComponent(cellUIs[j]);
                    gridBlock.SetCell(x, y, cell);
                }

                container.SetGridBlock(i, gridBlock);
            }

            _containerUIDic[containerGuid] = container;
            _scrollContentContainer.Add(container.RootElement);
        }

        public void RemoveContainer(Guid containerGuid)
        {
            _containerUIDic.Remove(containerGuid);
        }

        public void SetSelectingGridBlockIndex(int gridBlockIndex)
        {
            _selectingGridBlockIndex = gridBlockIndex;
        }

        public void SetSelectingContainerGuid(Guid containerGuid)
        {
            _selectingContainerGuid = containerGuid;
        }
    }
}