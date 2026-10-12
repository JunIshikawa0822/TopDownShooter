using System;
using UnityEngine;
using Game.Data;

public class Container
{
    private readonly Guid _containerDataGuid;
    private readonly GridBlock[] _gridBlocks;
    private float _currentWeight;
    public int GridBlockCount => _gridBlocks.Length;
    public Guid Guid => _containerDataGuid;

    public float CurrentWeight => _currentWeight;
    public Action ContentChanged;
    public Container(ContainerData containerData)
    {
        GridBlockData[] gridBlockDatas = containerData.ContainerBuild;
        _gridBlocks = new GridBlock[gridBlockDatas.Length];

        for (int i = 0; i < gridBlockDatas.Length; ++i)
        {
            _gridBlocks[i] = new GridBlock(gridBlockDatas[i].width, gridBlockDatas[i].height);
        }

        _containerDataGuid = Guid.NewGuid();
    }

    public bool TryPlaceItem(int gridBlockIndex, InventoryItemData item, int x, int y, ItemDirection dir)
    {
        if (!IsValidBlockIndex(gridBlockIndex)) return false;
        if (!_gridBlocks[gridBlockIndex].CanPlace(item, x, y, dir)) return false;

        item.SetItemOrigin(gridBlockIndex, x, y);
        item.SetDirection(dir);

        _gridBlocks[gridBlockIndex].PlaceToGrid(item, x, y, dir);

        _currentWeight += item.RuntimeData.CurrentWeight;

        return true;
    }

    public bool TryRemoveItem(int gridBlockIndex, InventoryItemData item)
    {
        if (!IsValidBlockIndex(gridBlockIndex)) return false;
        if (!_gridBlocks[gridBlockIndex].IsContain(item)) return false;

        item.ClearItemOrigin();

        _gridBlocks[gridBlockIndex].RemoveFromGrid(item);
        _currentWeight -= item.RuntimeData.BaseData.Weight * item.RuntimeData.StackCount;
        return true;
    }

    public bool TryRemoveItem(int gridBlockIndex, int x, int y)
    {
        if (!IsValidBlockIndex(gridBlockIndex)) return false;
        //当該マスにアイテムがあるかを確認
        InventoryItemData item = _gridBlocks[gridBlockIndex].TryFindItem(x, y);

        return TryRemoveItem(gridBlockIndex, item);
    }

    private bool IsValidBlockIndex(int gridBlockIndex)
    {
        bool result = gridBlockIndex >= 0 && gridBlockIndex < _gridBlocks.Length;
        if (result == false) Debug.LogError("Indexがおかしいですよ");

        return result;
    }

    /// <summary>
    /// コンテナ全体からアイテムの空きスペースを探す
    /// </summary>
    /// <param name="itemData">配置したいアイテムのデータ</param>
    /// <param name="allowRotation">回転を考慮するか</param>
    public bool TryFindSpace(InventoryItemData itemData, bool allowRotation, out int blockIndex, out int x, out int y, out ItemDirection dir)
    {
        blockIndex = -1;
        x = -1;
        y = -1;
        dir = ItemDirection.Up;

        // 元のサイズを取得
        int w = itemData.RuntimeData.BaseData.VisualData.Width;
        int h = itemData.RuntimeData.BaseData.VisualData.Height;

        for (int i = 0; i < _gridBlocks.Length; i++)
        {
            if (_gridBlocks[i].TryFindEmptySpot(w, h, allowRotation, out int fx, out int fy, out ItemDirection fDir))
            {
                blockIndex = i;
                x = fx;
                y = fy;
                dir = fDir;
                return true;
            }
        }

        return false;
    }
}
