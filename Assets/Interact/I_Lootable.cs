public interface ILootable : IInteractable
{
    int LootableID{get;}
    LootableType LootableType{get;}
    void SetID(int id);
    void SetLootableType(LootableType lootableType);
}
