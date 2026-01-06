using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;

namespace Game.UI
{
    public class GridBlockComponent
    {
        private VisualElement _gridBlockRoot;
        private VisualElement _itemParent;
        private readonly int _gridBlockIndex;
        private readonly int _gridWidth;
        private readonly int _gridHeight;
        private readonly int _cellSize;
        private readonly HashSet<GridCellComponent> _cells = new();
        private readonly Dictionary<(int x, int y), GridCellComponent> _cellsDic = new();
        private readonly HashSet<InventoryItemComponent> _items = new();
        public Action<int> OnPointerEnterEvent;

        public GridBlockComponent(int gridBlockIndex, int width, int height, int cellSize)
        {
            _gridBlockIndex = gridBlockIndex;
            _gridWidth = width;
            _gridHeight = height;
            _cellSize = cellSize;
        }
        public void SetVisualElements(VisualElement visualElement)
        {
            if(visualElement == null)return;
            _gridBlockRoot = visualElement;
            _itemParent = visualElement.Q("gridblock__itemparent");
        }

        public void RegisterPointerCallbacks()
        {
            _gridBlockRoot.RegisterCallback<PointerEnterEvent>(OnPointerEnter);
            _gridBlockRoot.RegisterCallback<PointerLeaveEvent>(OnPointerLeave);
        }

        public void UnregisterPointerCallbacks()
        {
            _gridBlockRoot.UnregisterCallback<PointerEnterEvent>(OnPointerEnter);
            _gridBlockRoot.UnregisterCallback<PointerLeaveEvent>(OnPointerLeave);
        }

        public void SetCell(int x, int y, GridCellComponent cell)
        {
            _cells.Add(cell);
            _cellsDic[(x, y)] = cell;
        }

        public void PlaceItem(int x, int y, int rotationDeg, InventoryItemComponent item)
        {
            item.Icon.style.position = Position.Absolute;
            // 座標変換（左上基準）
            item.Icon.style.left = x * _cellSize;
            item.Icon.style.top  = y * _cellSize;

            // 回転（度数指定）
            item.Icon.style.rotate = new Rotate(rotationDeg);

            // まだ追加されていなければ追加
            if (item.Icon.parent != _gridBlockRoot)
            {
                _gridBlockRoot.Add(item.Icon);
            }

            _items.Add(item);
        }
        public void RemoveCell(int x, int y)
        {
            _cells.Remove(_cellsDic[(x, y)]);
            _cellsDic.Remove((x, y));
        }
        public void RemoveItem(InventoryItemComponent item)
        {
            _items.Remove(item);
        }
        public void SetColor(Color color)
        {
            _gridBlockRoot.style.backgroundColor = new StyleColor(color);
        }
        public void OnPointerEnter(PointerEnterEvent evt)
        {
            OnPointerEnterEvent?.Invoke(_gridBlockIndex);
        }

        public void OnPointerLeave(PointerLeaveEvent evt)
        {

        }
    }
}