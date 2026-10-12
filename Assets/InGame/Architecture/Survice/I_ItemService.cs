using Game.Data;

public interface IItemService
{
    public InventoryItemData CreateNewItemRuntime(ItemData data, int amount);
}