using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory : StorageBase
{
    [SerializeField] private int _maxCapacity;
    [SerializeField] private List<ResourceView> _items = new();


    public override bool CanAdd(ResourceType type = ResourceType.N1)
    {
        return _items.Count < _maxCapacity;
    }

    public override bool HasItems(List<ResourceModel> ingredients)
    {
        foreach (var item in ingredients)
        {
            if (_items.Count(x => x.Type == item.Type) < item.Amount)
                return false;
        }

        return true;
    }

    public override bool HasItem(ResourceModel ingridient)
    {
        return _items.Count(x => x.Type == ingridient.Type) >= ingridient.Amount;
    }

    public override bool TryAdd(ResourceView item)
    {
        if (!CanAdd())
            return false;

        if (_items.Contains(item))
            return false;

        AddItem(item);
        return true;
    }

    public void AddWithoutChecking(ResourceView item)
    {
        AddItem(item);
    }

    public override bool TryGet(ResourceType type, out ResourceView item)
    {
        int lastIndex = _items.FindLastIndex(x => x.Type == type);
        if (lastIndex == -1)
        {
            item = null;
            return false;
        }

        item = _items[lastIndex];
        RemoveItem(item);

        return true;
    }

    public override bool TryGetWithoutRemoving(ResourceModel itemModel, out List<ResourceView> output)
    {
        if (!HasItem(itemModel))
        {
            output = null;
            return false;
        }

        output = GetItems(itemModel);
        return true;
    }

    protected override void RemoveItem(ResourceView item)
    {
        if (_items.Remove(item))
        {
            base.RemoveItem(item);
        }
    }

    protected override void AddItem(ResourceView item)
    {
        _items.Add(item);
        base.AddItem(item);
    }

    private List<ResourceView> GetItems(ResourceModel itemModel)
    {
        List<ResourceView> items = _items.GetRange(_items.Count - itemModel.Amount, itemModel.Amount);

        return items;
    }
}