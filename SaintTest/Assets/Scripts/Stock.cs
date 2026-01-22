using System.Collections.Generic;
using UnityEngine;

public class Stock : StorageBase
{
    [SerializeField] private int _capacity = 10;
    [SerializeField] private List<ResourceType> _acceptedTypes;

    private readonly SortedDictionary<ResourceType, List<ResourceView>> _itemsByType = new();
    private readonly Dictionary<ResourceType, int> _capacityPerType = new();

    public IReadOnlyList<ResourceType> AcceptedTypes => _acceptedTypes;

    protected override void Awake()
    {
        base.Awake();
        foreach (var type in _acceptedTypes)
        {
            _itemsByType[type] = new List<ResourceView>();
            _capacityPerType[type] = _capacity / _acceptedTypes.Count;
        }
    }

    public override bool CanAdd(ResourceType type)
    {
        if (!_itemsByType.TryGetValue(type, out var value))
        {
            return false;
        }

        return value.Count < _capacityPerType[type];
    }

    public override bool HasItems(List<ResourceModel> ingridients)
    {
        foreach (var item in ingridients)
        {
            if (_itemsByType[item.Type].Count < item.Amount)
                return false;
        }

        return true;
    }

    public override bool HasItem(ResourceModel ingridient)
    {
        return _itemsByType[ingridient.Type].Count >= ingridient.Amount;
    }

    public override bool TryAdd(ResourceView item)
    {
        var type = item.Type;
        if (!CanAdd(type))
            return false;

        AddItem(item);
        return true;
    }

    public override bool TryGet(ResourceModel itemModel, out List<ResourceView> items)
    {
        if (!HasItem(itemModel))
        {
            items = null;
            return false;
        }

        items = GetItems(itemModel);
        return true;
    }

    public override bool TryGet(ResourceType itemType, out ResourceView item)
    {
        if (!_itemsByType.TryGetValue(itemType, out var itemsType))
        {
            item = null;
            return false;
        }

        item = itemsType[^1];
        RemoveItem(item);

        return true;
    }

    private List<ResourceView> GetItems(ResourceModel itemModel)
    {
        List<ResourceView> items = _itemsByType[itemModel.Type]
            .GetRange(_itemsByType[itemModel.Type].Count - itemModel.Amount, itemModel.Amount);

        foreach (var item in items)
        {
            RemoveItem(item);
        }

        return items;
    }

    protected override void RemoveItem(ResourceView item)
    {
        if (_itemsByType[item.Type].Remove(item))
            base.RemoveItem(item);
    }

    protected override void AddItem(ResourceView item)
    {
        _itemsByType[item.Type].Add(item);
        base.AddItem(item);
    }


    private bool HasItem(ResourceType type)
    {
        return _itemsByType[type].Count > 0;
    }
}