using UnityEngine;
using System;
using System.Collections.Generic; 
using System.Linq;

public class GridBlock
{
    private readonly InventoryItemData[,] _grid;
    private readonly HashSet<InventoryItemData> _items = new();
    private readonly Dictionary<Guid, InventoryItemData> _guidDic = new();

    private int _gridBlockIndex;
    private int _gridWidth;
    private int _gridHeight;
    private float _currentWeight;

    public int GridIndex => _gridBlockIndex;
    public int GridWidth => _gridWidth;
    public int GridHeight => _gridHeight;
    public float CurrentWeight => _currentWeight;
    public Action<GridBlock> ContentChanged;
    public InventoryItemData[] AllItemsData => _items.ToArray();

    public GridBlock(int gridIndex, int width, int height)
    {
        _gridBlockIndex = gridIndex;
        _gridWidth = width;
        _gridHeight = height;
        _currentWeight = 0;

        _grid = new InventoryItemData[width, height];
    }

    public InventoryItemData[] TryFindItem(string itemID)
    {
        InventoryItemData[] items = _items.Where(item => item.RuntimeData.BaseData.ID == itemID).ToArray();

        if(items.Length == 0) return null;
        return items;
    }

    public InventoryItemData TryFindItem(Guid itemDataGuid)
    {
        InventoryItemData item = _guidDic[itemDataGuid];
        if(item == null) return null;

        return item;
    }

    public bool TryPlaceItem(InventoryItemData item, int originX, int originY, ItemDirection dir)
    {
        if (!CanPlace(item, originX, originY, dir)) return false;

        PlaceToGrid(item, originX, originY, dir);
        _currentWeight += item.RuntimeData.BaseData.Weight * item.RuntimeData.StackCount;

        ContentChanged?.Invoke(this);
        return true;
    }

    public bool TryRemoveItem(InventoryItemData item)
    {
        if (!_items.Contains(item)) return false;

        RemoveFromGrid(item);
        _currentWeight -= item.RuntimeData.BaseData.Weight * item.RuntimeData.StackCount;

        ContentChanged?.Invoke(this);
        return true;
    }

    public bool TryRemoveItem(int x, int y)
    {
        InventoryItemData item = _grid[x, y];
        if(item == null) return false;

        return TryRemoveItem(item);
    }

    private bool CanPlace(InventoryItemData item, int x, int y, ItemDirection dir)
    {
        foreach ((int cellX, int cellY) in item.CalculateOccupiedCells(x, y, dir))
        {
            if (cellX < 0 || cellY < 0 || cellX >= _gridWidth || cellY >= _gridHeight) return false;
            if (_grid[cellX, cellY] != null && _grid[cellX, cellY] != item) return false;
        }
        return true;
    }

    private void RemoveFromGrid(InventoryItemData item)
    {
        foreach ((int cellX, int cellY) in item.GetOccupiedCells())
        {
            if (_grid[cellX, cellY] != item)
            {
                // 設計破綻の検出用
                Debug.LogWarning($"Grid mismatch at ({cellX},{cellY})");
                continue;
            }
            _grid[cellX, cellY] = null;
        }

        _guidDic.Remove(item.ItemDataGuid);
        _items.Remove(item);
    }

    private void PlaceToGrid(InventoryItemData item, int originX, int originY, ItemDirection dir)
    {
        item.SetItemOrigin(_gridBlockIndex, originX, originY);
        item.SetDirection(dir);

        foreach ((int x, int y) in item.GetOccupiedCells()) _grid[x, y] = item;

        _guidDic[item.ItemDataGuid] = item;
        _items.Add(item);
    }
}
