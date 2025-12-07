using UnityEngine;
using UnityEngine.UIElements;
using System.Collections;
using System.Collections.Generic; 

public class Inventory
{
    private int _gridWidth;
    private int _gridHeight;
    private float _maxWeight;
    private float _currentWeight;

    private readonly InventoryItemData[,] _grid;
    private readonly HashSet<InventoryItemData> _items = new();

    public int GridWidth => _gridWidth;
    public int GridHeight => _gridHeight;
    public float CurrentWeight => _currentWeight;
    public float MaxWeight => _maxWeight;

    public Inventory(int width, int height, float maxWeight = 0)
    {
        _gridWidth = width;
        _gridHeight = height;
        _maxWeight = maxWeight;
        _currentWeight = 0;

        _grid = new InventoryItemData[width, height];
    }

    public bool TryAddItem(InventoryItemData item, int originX, int originY, ItemDirection dir)
    {
        if (!CanPlace(item, originX, originY, dir)) return false;

        PlaceItem(item, originX, originY, dir);
        _items.Add(item);

        _currentWeight += item.RuntimeData.BaseData.Weight * item.RuntimeData.StackCount;

        // NotifyItemAdded(item);
        // NotifyWeightChanged();

        return true;
    }

    public InventoryItemData TryRemoveItem(InventoryItemData item)
    {
        if(item == null) return null;

        RemoveFromGrid(item);
        return item;
    }

    private void RemoveFromGrid(InventoryItemData item)
    {
        foreach ((int x, int y) in item.GetOccupiedCells())
        {
            if (_grid[x, y] == item) _grid[x, y] = null;
        }
    }

    private bool CanPlace(InventoryItemData item, int x, int y, ItemDirection dir)
    {
        foreach ((int cellX, int cellY) in item.CalculateOccupiedCells(x, y, dir))
        {
            if (cellX < 0 || cellY < 0 || cellX >= _gridWidth || cellY >= _gridHeight)
                return false;
            if (_grid[cellX, cellY] != null && _grid[cellX, cellY] != item)
                return false;
        }
        return true;
    }

    private void PlaceItem(InventoryItemData item, int originX, int originY, ItemDirection dir)
    {
        item.SetItemOrigin(originX, originY);
        item.SetDirection(dir);

        foreach ((int x, int y) in item.GetOccupiedCells())
            _grid[x, y] = item;
    }

    private bool IsOverlap(InventoryItemData a, InventoryItemData b)
    {
        IEnumerable<(int x, int y)> aCells = a.GetOccupiedCells();
        IEnumerable<(int x, int y)> bCells = b.GetOccupiedCells();

        foreach ((int x, int y) aCell in aCells)
            foreach ((int x, int y) bCell in bCells)
                if (aCell == bCell) return true;

        return false;
    }
}

public enum PlaceResult {
    Success,
    OutOfBounds,
    Overlap,
    InvalidRotation,
    OtherError
}

