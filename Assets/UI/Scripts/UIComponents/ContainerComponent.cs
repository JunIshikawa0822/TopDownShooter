using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Game.UI
{
    public class ContainerComponent
    {
        private VisualElement _containerRoot;
        private VisualElement _containerText;
        private readonly Dictionary<int, GridBlockComponent> _gridMap = new();
        private readonly Guid _containerGuid;
        public event Action<Guid> OnPointerEnterEvent;
        public event Action<Guid> OnPointerLeaveEvent;
        //public VisualElement RootElement => _containerRoot;
        public ContainerComponent(Guid containerDataGuid)
        {
            _containerGuid = containerDataGuid;
        }
        public void SetVisualElements(VisualElement containerElement)
        {
            if(containerElement == null)return;
            _containerRoot = containerElement.Q("containeritem__root");
            _containerText = containerElement.Q("containeritem__header-text");
        }

        public void RegisterPointerCallbacks()
        {
            _containerRoot.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
            _containerRoot.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
        }

        public void UnregisterPointerCallbacks()
        {
            _containerRoot.UnregisterCallback<PointerEnterEvent>(OnPointerEnter);
            _containerRoot.UnregisterCallback<PointerLeaveEvent>(OnPointerLeave);
        }

        public void SetGridBlock(int index, GridBlockComponent gridBlock)
        {
            _gridMap[index] = gridBlock;
        }

        public void PlaceItem(int index, int x, int y, int rotationDeg, InventoryItemComponent itemElement)
        {
            _gridMap[index].PlaceItem(x, y, rotationDeg, itemElement);
        }

        public void RemoveItem(int index, InventoryItemComponent itemElement)
        {
            _gridMap[index].RemoveItem(itemElement);
        }

        public void SetColor(Color color)
        {
            _containerRoot.style.backgroundColor = new StyleColor(color);
        }

        public void OnPointerEnter(PointerEnterEvent evt)
        {
            OnPointerEnterEvent?.Invoke(_containerGuid);
        }

        public void OnPointerLeave(PointerLeaveEvent evt)
        {
            OnPointerLeaveEvent?.Invoke(_containerGuid);
        }

        // private int ReadWidthFromClass(VisualElement block)
        // {
        //     foreach (string className in block.GetClasses())
        //     {
        //         //col-から始まるタグを取得、4文字目以降を変換する
        //         if (className.StartsWith("col-") && int.TryParse(className.Substring(4), out int columns))
        //         {
        //             return columns;
        //         }
        //     }

        //     //指定されていない場合の保険
        //     return 1;
        // }

        // public void SetCellsColor(int gridBlockId, Vector2[] cells, Color color)
        // {
        //     foreach(Vector2 cell in cells)
        //     {
        //         SetCellColor(gridBlockId, (int)cell.x, (int)cell.y, color);
        //     }
        // }

        // public void SetCellColor(int gridBlockId, int x, int y, Color color)
        // {
        //     if (!_gridMap.TryGetValue(gridBlockId, out var grid)) return;

        //     if (x < 0 || y < 0 || x >= grid.GetLength(0) || y >= grid.GetLength(1)) return;

        //     GridCellComponent cell = grid[x, y];
        //     if (cell == null) return;

        //     cell.SetColor(color);
        // }

        // private void RegisterButtonCallbacks()
        // {
        //     foreach(VisualElement gridBlock in _gridBlocks){
        //         gridBlock.RegisterCallback<PointerEnterEvent>(OnGridEnter);
        //         gridBlock.RegisterCallback<PointerLeaveEvent>(OnGridLeave);
        //     }

        //     foreach(VisualElement cell in _cells){
        //         cell.RegisterCallback<PointerEnterEvent>(OnCellEnter);
        //         cell.RegisterCallback<PointerLeaveEvent>(OnCellLeave);
        //     }
        // }

        // private void UnregisterButtonCallbacks()
        // {
        //     foreach(VisualElement gridBlock in _gridBlocks){
        //         gridBlock.UnregisterCallback<PointerEnterEvent>(OnGridEnter);
        //         gridBlock.UnregisterCallback<PointerLeaveEvent>(OnGridLeave);
        //     }

        //     foreach(VisualElement cell in _cells){
        //         cell.UnregisterCallback<PointerEnterEvent>(OnCellEnter);
        //         cell.UnregisterCallback<PointerLeaveEvent>(OnCellLeave);
        //     }
        // }

        // public void OnDisable()
        // {
        //     UnregisterButtonCallbacks();
        // }
    }
}