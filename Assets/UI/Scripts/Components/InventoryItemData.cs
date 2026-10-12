using Game.Data;
using System.Collections.Generic;
using System;

public class InventoryItemData
{
    private int _gridIndex;
    private int _originX;
    private int _originY;
    private IItemRuntime _itemRuntime;
    private ItemDirection _direction;
    private readonly Guid _itemDataGuid;

    public IItemRuntime Runtime => _itemRuntime;
    public int GridIndex => _gridIndex;
    public int OriginX => _originX;
    public int OriginY => _originY;
    public int Width => Runtime.VisualData.Width;
    public int Height => Runtime.VisualData.Height;
    public ItemDirection Direction => _direction;
    public int DirectionDeg => _direction == ItemDirection.Up ? 0 : 90;
    public Guid ItemDataGuid => _itemDataGuid;

    public InventoryItemData(int gridIndex, int originX, int originY, IItemRuntime itemRuntime)
    {
        _gridIndex = gridIndex;
        _originX = originX;
        _originY = originY;
        _itemRuntime = itemRuntime;
        _direction = ItemDirection.Up;

        _itemDataGuid = Guid.NewGuid();
    }

    public void SetItemOrigin(int gridIndex, int originX, int originY)
    {
        _gridIndex = gridIndex;
        _originX = originX;
        _originY = originY;
    }

    public void ClearItemOrigin()
    {
        _gridIndex = -1;
        _originX = -1;
        _originY = -1;
    }

    public void SetDirection(ItemDirection direction)
    {
        _direction = direction;
    }

    public IEnumerable<(int x, int y)> GetOccupiedCells()
    {
        // 回転方向に応じてサイズを入れ替え
        int width = _direction == ItemDirection.Up ? Runtime.VisualData.Width : Runtime.VisualData.Height;
        int height = _direction == ItemDirection.Up ? Runtime.VisualData.Height : Runtime.VisualData.Width;

        for (int dx = 0; dx < width; ++dx)
        {
            for (int dy = 0; dy < height; ++dy)
            {
                yield return (_originX + dx, _originY + dy);
            }
        }
    }

    public IEnumerable<(int x, int y)> CalculateOccupiedCells(int originX, int originY, ItemDirection direction)
    {
        // 回転方向に応じてサイズを入れ替え
        int width = direction == ItemDirection.Up ? Runtime.VisualData.Width : Runtime.VisualData.Height;
        int height = direction == ItemDirection.Up ? Runtime.VisualData.Height : Runtime.VisualData.Width;

        for (int dx = 0; dx < width; ++dx)
        {
            for (int dy = 0; dy < height; ++dy)
            {
                yield return (originX + dx, originY + dy);
            }
        }
    }

    public override bool Equals(object obj)
    {
        return obj is InventoryItemData other && _itemDataGuid == other._itemDataGuid;
    }

    public override int GetHashCode()
    {
        return _itemDataGuid.GetHashCode();
    }
}

public enum ItemDirection
{
    Up,
    Right
}
