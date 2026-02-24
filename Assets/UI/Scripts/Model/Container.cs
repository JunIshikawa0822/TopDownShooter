using System;
using UnityEngine;
using Game.Data;
using System.Linq;
using System.Collections.Generic;

public class Container
{
    private readonly Guid _containerDataGuid;
    private readonly GridBlock[] _gridBlocks;
    private float _currentWeight;
    private ContainerData _containerData;
    public int GridBlockCount => _gridBlocks.Length;
    public Guid Guid => _containerDataGuid;
    public float CurrentWeight => _currentWeight;
    public GridBlock[] GridBlocks => _gridBlocks;
    public Action ContentChanged;
    public ContainerData ContainerData => _containerData;
    public Container(ContainerData containerData)
    {
        _containerData = containerData;
        GridBlockData[] gridBlockDatas = containerData.ContainerBuild;
        _gridBlocks = new GridBlock[gridBlockDatas.Length];

        for (int i = 0; i < gridBlockDatas.Length; ++i)
        {
            _gridBlocks[i] = new GridBlock(gridBlockDatas[i].width, gridBlockDatas[i].height);
        }

        _containerDataGuid = Guid.NewGuid();
    }

    public bool TryPlaceItem(InventoryItemData item, int gridBlockIndex, int x, int y, ItemDirection dir)
    {
        if (!IsValidBlockIndex(gridBlockIndex)) return false;
        if (!_gridBlocks[gridBlockIndex].CanPlace(item, x, y, dir)) return false;

        item.SetItemOrigin(gridBlockIndex, x, y);
        item.SetDirection(dir);

        _gridBlocks[gridBlockIndex].PlaceToGrid(item, x, y, dir);
        _currentWeight += item.Runtime.Weight;

        ContentChanged?.Invoke();
        return true;
    }

    public bool TryRemoveItem(InventoryItemData item, int gridBlockIndex)
    {
        if (!IsValidBlockIndex(gridBlockIndex)) return false;
        if (!_gridBlocks[gridBlockIndex].IsContain(item)) return false;

        item.ClearItemOrigin();

        _gridBlocks[gridBlockIndex].RemoveFromGrid(item);
        _currentWeight -= item.Runtime.Weight;
        ContentChanged?.Invoke();
        return true;
    }

    public bool TryRemoveItem(int gridBlockIndex, int x, int y)
    {
        if (!IsValidBlockIndex(gridBlockIndex)) return false;
        //当該マスにアイテムがあるかを確認
        InventoryItemData item = _gridBlocks[gridBlockIndex].TryFindItem(x, y);

        return TryRemoveItem(item, gridBlockIndex);
    }

    private bool IsValidBlockIndex(int gridBlockIndex)
    {
        bool result = gridBlockIndex >= 0 && gridBlockIndex < _gridBlocks.Length;
        if (result == false) Debug.LogError("Indexがおかしいですよ");

        return result;
    }

    /// <summary>
    /// 空いているスペースを自動で探し、アイテムを配置する
    /// </summary>
    /// <param name="itemData">配置したいアイテム</param>
    /// <param name="allowRotation">回転を許可するか</param>
    /// <returns>配置に成功したか</returns>
    public bool TryAutoPlace(InventoryItemData itemData, bool allowRotation)
    {
        if (TryFindSpace(itemData, allowRotation, out int blockIndex, out int x, out int y, out ItemDirection dir))
        {
            return TryPlaceItem(itemData, blockIndex, x, y, dir);
        }

        return false;
    }

    /// <summary>
    /// コンテナ全体からアイテムの空きスペースを探す
    /// </summary>
    private bool TryFindSpace(InventoryItemData itemData, bool allowRotation, out int blockIndex, out int x, out int y, out ItemDirection dir)
    {
        blockIndex = -1;
        x = -1;
        y = -1;
        dir = ItemDirection.Up;

        // 元のサイズを取得
        int w = itemData.Runtime.VisualData.Width;
        int h = itemData.Runtime.VisualData.Height;

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
