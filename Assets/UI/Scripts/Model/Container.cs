using System;
using System.Linq;

public class Container
{
    private readonly Guid _containerDataGuid;
    private readonly GridBlock[] _gridBlocks;
    public int GridBlockCount => _gridBlocks.Length;
    public Guid Guid => _containerDataGuid;
    public Container(ContainerData containerData)
    {
        GridBlockData[] gridBlockDatas = containerData.ContainerBuild;
        _gridBlocks = new GridBlock[gridBlockDatas.Length];

        for(int i = 0; i < gridBlockDatas.Length; ++i)
        {
            _gridBlocks[i] = new GridBlock(i, gridBlockDatas[i].width, gridBlockDatas[i].height);
        }

        _containerDataGuid = Guid.NewGuid();
    }

    public InventoryItemData[] TryFindItems(int gridBlockIndex, string itemID)
    {
        return _gridBlocks[gridBlockIndex].TryFindItem(itemID);
    }

    public InventoryItemData TryFindItem(int gridBlockIndex, Guid guid)
    {
        return _gridBlocks[gridBlockIndex].TryFindItem(guid);
    }

    public bool TryPlaceItem(int gridBlockIndex, InventoryItemData item, int x, int y, ItemDirection dir)
    {
        return _gridBlocks[gridBlockIndex].TryPlaceItem(item, x, y, dir);
    }

    public bool TryRemoveItem(int gridBlockIndex, InventoryItemData item)
    {
        return _gridBlocks[gridBlockIndex].TryRemoveItem(item);
    }

    public bool TryRemoveItem(int gridBlockIndex, int x, int y)
    {
        return _gridBlocks[gridBlockIndex].TryRemoveItem(x, y);
    }

    public InventoryItemData[] GetItemsData(int index)
    {
        return _gridBlocks[index].AllItemsData;
    }
}
