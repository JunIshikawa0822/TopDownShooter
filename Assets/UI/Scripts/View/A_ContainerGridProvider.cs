using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace Game.UI
{
    public abstract class AContainerGridProvider : AUIView
    {
        protected const int _cellSize = 48;
        protected Guid _selectingContainerGuid;
        protected int _selectingGridBlockIndex = 0;
        protected readonly Dictionary<Guid, ContainerComponent> _containerUIDic = new();

        protected virtual GridCellComponent CreateGridCellComponent(VisualElement cellElement)
        {
            GridCellComponent cellComponent = new();
            cellComponent.SetVisualElements(cellElement);
            //セルにマウスが入ってきたときどんな挙動をする？OnPointerEnterの登録
            //セルからマウスが離れたときどんな挙動をする？OnPointerLeaveの登録
            cellComponent.RegisterPointerCallbacks();
            return cellComponent;
        }

        protected virtual GridBlockComponent CreateGridBlockComponent(VisualElement gridBlockElement, int gridBlockIndex, GridBlockData gridBlockData, int cellSize)
        {
            GridBlockComponent gridBlockComponent = new GridBlockComponent(gridBlockIndex, gridBlockData.width, gridBlockData.height, cellSize);
            gridBlockComponent.SetVisualElements(gridBlockElement);
            gridBlockComponent.OnPointerEnterEvent += SetSelectingGridBlockIndex;//OnPointerEnterの登録

            gridBlockComponent.RegisterPointerCallbacks();
            return gridBlockComponent;
        }

        protected virtual ContainerComponent CreateContainerComponent(VisualElement containerElement, Guid containerGuid)
        {
            ContainerComponent containerComponent = new ContainerComponent(containerGuid);
            containerComponent.SetVisualElements(containerElement);
            //OnPointerEnterの登録
            containerComponent.OnPointerEnterEvent += SetSelectingContainerGuid;
            //OnPointerLeaveの登録
            containerComponent.RegisterPointerCallbacks();
            return containerComponent;
        }

        //ContainerのUIに関する初期化のクラス
        public virtual void AddContainer(VisualElement containerElementRoot, GridBlockData[] gridBlockDatas, Guid containerGuid)
        {
            InitContainer(containerElementRoot, gridBlockDatas, containerGuid);
        }

        public virtual void RemoveContainer(Guid containerGuid)
        {
            _containerUIDic.Remove(containerGuid);
        }

        protected virtual void InitContainer(VisualElement containerElementRoot, GridBlockData[] gridBlockDatas, Guid containerGuid)
        {
            List<VisualElement> gridBlockUIs = containerElementRoot.Query<VisualElement>(className: "GridBlock").ToList();

            if (gridBlockDatas.Length != gridBlockUIs.Count)
            {
                Debug.LogError("データとUIが異なります");
                return;
            }

            //Containerを表現するUIクラスの作成
            ContainerComponent container = CreateContainerComponent(containerElementRoot, containerGuid);

            //GridBlockのUIに関する初期化のクラス
            for (int i = 0; i < gridBlockDatas.Length; ++i)
            {
                int count = gridBlockDatas[i].width * gridBlockDatas[i].height;
                List<VisualElement> cellUIs = gridBlockUIs[i].Query<VisualElement>(className: "GridCell").ToList();

                //Debug.Log($"{gridBlockUIs.Count}");
                //Debug.Log($"{cellUIs.Count}個のcell");
                if (count != cellUIs.Count)
                {
                    Debug.LogError($"{i}番めのgridblockでデータとUIが異なります");
                    return;
                }

                GridBlockComponent gridBlock = CreateGridBlockComponent(gridBlockUIs[i], i, gridBlockDatas[i], _cellSize);

                //セルUIを初期化
                for (int j = 0; j < count; ++j)
                {
                    int x = count % gridBlockDatas[i].width;
                    int y = count / gridBlockDatas[i].height;
                    GridCellComponent cell = CreateGridCellComponent(cellUIs[j]);
                    gridBlock.SetCell(x, y, cell);
                }

                container.SetGridBlock(i, gridBlock);
            };

            //ContainerにCellやGridBlockの情報を渡し、Guidと結びつけるまで
            _containerUIDic[containerGuid] = container;
        }

        protected virtual void SetSelectingGridBlockIndex(int gridBlockIndex)
        {
            _selectingGridBlockIndex = gridBlockIndex;
        }

        protected virtual void SetSelectingContainerGuid(Guid containerGuid)
        {
            _selectingContainerGuid = containerGuid;
        }
    }
}
