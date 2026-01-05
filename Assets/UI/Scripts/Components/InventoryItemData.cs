using Game.Data;
using System.Collections.Generic;
using System;

public class InventoryItemData
{
    private int _gridIndex;
    private int _originX;
    private int _originY;
    private IItemRuntimeData _itemRuntimeData;
    private ItemDirection _direction;
    private readonly Guid _itemDataGuid;

    public IItemRuntimeData RuntimeData => _itemRuntimeData;
    public int GridIndex => _gridIndex;
    public int OriginX => _originX;
    public int OriginY => _originY;
    public ItemDirection Direction => _direction;
    public Guid ItemDataGuid => _itemDataGuid;

    public InventoryItemData(int gridIndex, int originX, int originY, IItemRuntimeData itemRuntimeData)
    {
        _gridIndex = gridIndex;
        _originX = originX;
        _originY = originY;
        _itemRuntimeData = itemRuntimeData;
        _direction = ItemDirection.Up;

        _itemDataGuid = Guid.NewGuid();
    }

    public void SetItemOrigin(int gridIndex, int originX, int originY)
    {
        _gridIndex = gridIndex;
        _originX = originX;
        _originY = originY;
    }

    public void SetDirection(ItemDirection direction)
    {
        _direction = direction;
    }

    public IEnumerable<(int x, int y)> GetOccupiedCells()
    {
        // 回転方向に応じてサイズを入れ替え
        int width  = _direction == ItemDirection.Up ? RuntimeData.BaseData.VisualData.Width  :  RuntimeData.BaseData.VisualData.Height;
        int height = _direction == ItemDirection.Up ? RuntimeData.BaseData.VisualData.Height : RuntimeData.BaseData.VisualData.Width;

        for (int dx = 0; dx < width; ++dx)
        {
            for (int dy = 0; dy < height; ++dy)
            {
                yield return (_originX+ dx, _originY + dy);
            }
        }
    } 

    public IEnumerable<(int x, int y)> CalculateOccupiedCells(int originX, int originY, ItemDirection direction)
    {
        // 回転方向に応じてサイズを入れ替え
        int width  = direction == ItemDirection.Up ? RuntimeData.BaseData.VisualData.Width  :  RuntimeData.BaseData.VisualData.Height;
        int height = direction == ItemDirection.Up ? RuntimeData.BaseData.VisualData.Height : RuntimeData.BaseData.VisualData.Width;

        for (int dx = 0; dx < width; ++dx)
        {
            for (int dy = 0; dy < height; ++dy)
            {
                yield return (originX + dx, originY + dy);
            }
        }
    }   

}

public enum ItemDirection
{
    Up,
    Right
}
