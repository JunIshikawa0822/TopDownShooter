using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class GridBlock
{
    private readonly InventoryItemData[,] _grid;
    private readonly HashSet<InventoryItemData> _items = new();
    private readonly Dictionary<Guid, InventoryItemData> _guidDic = new();
    private int _gridWidth;
    private int _gridHeight;
    public int GridWidth => _gridWidth;
    public int GridHeight => _gridHeight;

    public GridBlock(int width, int height)
    {
        _gridWidth = width;
        _gridHeight = height;

        _grid = new InventoryItemData[width, height];
    }

    public bool TryFindItem(string itemID, out InventoryItemData[] items)
    {
        InventoryItemData[] resultItems = _items.Where(item => item.RuntimeData.BaseData.ItemID == itemID).ToArray();
        bool result = resultItems.Length != 0;
        items = result ? resultItems : Array.Empty<InventoryItemData>();
        return result;
    }

    public bool TryFindItem(Guid itemDataGuid, out InventoryItemData item)
    {
        bool result = _guidDic.TryGetValue(itemDataGuid, out InventoryItemData resultItem);
        item = resultItem;
        return result;
    }

    public InventoryItemData TryFindItem(int x, int y)
    {
        if (x < 0 || y < 0 || x >= _gridWidth || y >= _gridHeight) return null;
        return _grid[x, y];
    }

    public bool CanPlace(InventoryItemData item, int x, int y, ItemDirection dir)
    {
        foreach ((int cellX, int cellY) in item.CalculateOccupiedCells(x, y, dir))
        {
            if (cellX < 0 || cellY < 0 || cellX >= _gridWidth || cellY >= _gridHeight)
            {
                Debug.LogWarning("範囲外にアクセスしました");
                return false;
            }

            if (_grid[cellX, cellY] != null && _grid[cellX, cellY] != item)
            {
                Debug.LogWarning("自身以外にアクセスしました");
                return false;
            }
        }
        return true;
    }

    public bool IsContain(InventoryItemData item)
    {
        if (item == null) return false;
        return _items.Contains(item);
    }

    //単純な排除作業
    public void RemoveFromGrid(InventoryItemData item)
    {
        if (item == null) return;

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

    public void RemoveFromGrid(int x, int y)
    {
        InventoryItemData item = TryFindItem(x, y);
        RemoveFromGrid(item);
    }

    //単純な挿入作業
    public void PlaceToGrid(InventoryItemData item, int originX, int originY, ItemDirection dir)
    {
        foreach ((int x, int y) in item.CalculateOccupiedCells(originX, originY, dir))
        {
            _grid[x, y] = item;
        }

        _guidDic[item.ItemDataGuid] = item;
        _items.Add(item);
    }

    /// <summary>
    /// 指定されたサイズの空きスペースを探す。
    /// </summary>
    /// <param name="allowRotation">回転を考慮するかどうか</param>
    public bool TryFindEmptySpot(int itemWidth, int itemHeight, bool allowRotation, out int x, out int y, out ItemDirection finalDir)
    {
        x = -1;
        y = -1;
        finalDir = ItemDirection.Up;

        // 1. デフォルトの向き (Up) で探索
        if (SearchAtDirection(itemWidth, itemHeight, out x, out y))
        {
            finalDir = ItemDirection.Up;
            return true;
        }

        // 2. 回転を許可する場合、横向き (Right) で探索
        if (allowRotation && itemWidth != itemHeight) // 正方形なら回転不要
        {
            if (SearchAtDirection(itemHeight, itemWidth, out x, out y))
            {
                finalDir = ItemDirection.Right;
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 特定の向き・サイズでグリッド内を走査する内部メソッド
    /// </summary>
    private bool SearchAtDirection(int searchWidth, int searchHeight, out int foundX, out int foundY)
    {
        foundX = -1;
        foundY = -1;

        // ループ範囲を最初からはみ出さない範囲に限定 (最適化)
        int endX = _gridWidth - searchWidth;
        int endY = _gridHeight - searchHeight;

        for (int j = 0; j <= endY; ++j)
        {
            for (int i = 0; i <= endX; ++i)
            {
                if (CheckAreaEmpty(i, j, searchWidth, searchHeight))
                {
                    foundX = i;
                    foundY = j;
                    return true;
                }
            }
        }
        return false;
    }

    /// <summary>
    /// 指定された範囲が完全に空いているかチェックする共通ロジック
    /// </summary>
    private bool CheckAreaEmpty(int startX, int startY, int itemWidth, int itemHeight)
    {
        // ループ範囲外のアクセスがないか念のためのガード
        if (startX < 0 || startY < 0 || startX + itemWidth > _gridWidth || startY + itemHeight > _gridHeight) return false;

        for (int dy = 0; dy < itemHeight; ++dy)
        {
            for (int dx = 0; dx < itemWidth; ++dx)
            {
                if (_grid[startX + dx, startY + dy] != null)
                {
                    return false;
                }
            }
        }
        return true;
    }
}
