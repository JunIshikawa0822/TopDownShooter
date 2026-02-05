using Game.Data;

public interface IItemService
{
    public IItemRuntime CreateNewItemRuntime(ItemData data, int amount);
}