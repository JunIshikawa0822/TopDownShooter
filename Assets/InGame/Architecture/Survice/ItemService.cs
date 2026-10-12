using UnityEngine;
using Game.Data;
using System.Collections.Generic;
using System;

public class ItemService : IItemService
{
    private readonly Dictionary<ItemType, Func<ItemData, int, Guid, IItemRuntime>> _typeDict;
    public ItemService()
    {
        _typeDict = new Dictionary<ItemType, Func<ItemData, int, Guid, IItemRuntime>>
        {
            { ItemType.Weapon, (data, amount, guid) => new WeaponRuntimeFactory().CreateNewItemRuntime(data, amount, guid)},
            //{ ItemType.Armor, (data, amount, guid) => new ArmorRuntime(data, amount, guid) },
            //{ ItemType.Accessory, (data, amount, guid) => new AccessoryRuntime(data, amount, guid) },
            { ItemType.Attachment, (data, amount, guid) => new AttachmentRuntimeFactory().CreateNewItemRuntime(data, amount, guid) },
            { ItemType.Ammo, (data, amount, guid) => new AmmoRuntime(data as AmmoData, amount, guid) },
            // { ItemType.Throwable, (data, amount, guid) => new ThrowableRuntime(data as ThrowableData, amount, guid) },
            // { ItemType.Container, (data, amount, guid) => new ContainerRuntime(data as ContainerData, amount, guid) },
            // { ItemType.Medical, (data, amount, guid) => new MedicalRuntime(data as MedicalData, amount, guid) },
            // { ItemType.Food, (data, amount, guid) => new FoodRuntime(data as FoodData, amount, guid) },
            // { ItemType.Drink, (data, amount, guid) => new DrinkRuntime(data as DrinkData, amount, guid) },
            // { ItemType.Stimulant, (data, amount, guid) => new StimulantRuntime(data as StimulantData, amount, guid) },
            // { ItemType.Material, (data, amount, guid) => new MaterialRuntime(data as MaterialData, amount, guid) },
            // { ItemType.Valuable, (data, amount, guid) => new ValuableRuntime(data as ValuableData, amount, guid) },
            // { ItemType.Intel, (data, amount, guid) => new IntelRuntime(data as IntelData, amount, guid) },
            // { ItemType.KeyItem, (data, amount, guid) => new KeyItemRuntime(data as KeyItemData, amount, guid) }
        };
    }

    public IItemRuntime CreateNewItemRuntime(ItemData data, int amount)
    {
        Guid guid = Guid.NewGuid();
        if (_typeDict.TryGetValue(data.ItemType, out Func<ItemData, int, Guid, IItemRuntime> createFunc))
        {
            return createFunc(data, amount, guid);
        }

        return null;
    }
}
