using System;
using UnityEngine;

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

        for(int i = 0; i < gridBlockDatas.Length; ++i)
        {
            _gridBlocks[i] = new GridBlock(gridBlockDatas[i].width, gridBlockDatas[i].height);
        }

        _containerDataGuid = Guid.NewGuid();
    }

    public bool TryPlaceItem(int gridBlockIndex, InventoryItemData item, int x, int y, ItemDirection dir)
    {
        if(!IsValidBlockIndex(gridBlockIndex)) return false;
        if(!_gridBlocks[gridBlockIndex].CanPlace(item, x, y, dir)) return false;

        item.SetItemOrigin(gridBlockIndex, x, y);
        item.SetDirection(dir);

        _gridBlocks[gridBlockIndex].PlaceToGrid(item, x, y, dir);

        _currentWeight += item.RuntimeData.BaseData.Weight * item.RuntimeData.StackCount;

        return true;
    }

    public bool TryRemoveItem(int gridBlockIndex, InventoryItemData item)
    {
        if(!IsValidBlockIndex(gridBlockIndex)) return false;
        if(!_gridBlocks[gridBlockIndex].IsContain(item)) return false;

        item.ClearItemOrigin();

        _gridBlocks[gridBlockIndex].RemoveFromGrid(item);
        _currentWeight -= item.RuntimeData.BaseData.Weight * item.RuntimeData.StackCount;
        return true;
    }

    public bool TryRemoveItem(int gridBlockIndex, int x, int y)
    {
        if(!IsValidBlockIndex(gridBlockIndex)) return false;
        //当該マスにアイテムがあるかを確認
        InventoryItemData item = _gridBlocks[gridBlockIndex].TryFindItem(x, y);
        
        return TryRemoveItem(gridBlockIndex, item);
    }

    private bool IsValidBlockIndex(int gridBlockIndex)
    {
        bool result = gridBlockIndex >= 0 && gridBlockIndex < _gridBlocks.Length;
        if(result == false) Debug.LogError("Indexがおかしいですよ");
        
        return result;
    }
}
