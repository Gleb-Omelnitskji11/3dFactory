using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : StorageBase
{
    [SerializeField] private int _capacity = 10;
    [SerializeField] private List<ResourceType> _acceptedTypes;
    [SerializeField] private bool _isOutput;
    private const float ItemMovingDelay = 0.5f;

    private readonly SortedDictionary<ResourceType, List<ResourceView>> _itemsByType = new();
    private readonly Dictionary<ResourceType, int> _capacityPerType = new();

    private bool _isPlayerInside;
    private PlayerInventory _movingInventory;

    public override object Items => _itemsByType;

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

        Add(item);
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

        int lastIndex = itemsType.FindLastIndex(x => x.Type == itemType);
        if (lastIndex == -1)
        {
            item = null;
            return false;
        }

        item = itemsType[lastIndex];
        Remove(item);

        return true;
    }

    private List<ResourceView> GetItems(ResourceModel itemModel)
    {
        List<ResourceView> items = _itemsByType[itemModel.Type].GetRange(_itemsByType[itemModel.Type].Count - itemModel.Amount, itemModel.Amount);

        foreach (var item in items)
        {
            Remove(item);
        }
        return items;
    }

    private void Remove(ResourceView item)
    {
        if(_itemsByType[item.Type].Remove(item))
            base.RemoveItem(item);
    }

    private void Add(ResourceView item)
    {
        _itemsByType[item.Type].Add(item);
        base.AddItem(item);
    }


    private bool HasItem(ResourceType type)
    {
        return _itemsByType[type].Count > 0;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerInventory inventory))
        {
            _movingInventory = inventory;
            _isPlayerInside = true;
            StartCoroutine(MoveItems());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerInventory inventory) &&
            inventory == _movingInventory)
        {
            _isPlayerInside = false;
            _movingInventory = null;
        }
    }

    private IEnumerator MoveItems()
    {
        while (_isPlayerInside)
        {
            bool success = _isOutput ? TryGiveItems() : TryTakeItems();

            yield return new WaitForSeconds(ItemMovingDelay);
        }
    }

    private bool TryGiveItems()
    {
        foreach (var type in _acceptedTypes)
        {
            if (!HasItem(type))
                continue;

            if (_movingInventory.CanAdd())
            {
                var item = _itemsByType[type][^1];
                Remove(item);
                if (!_movingInventory.TryAdd(item))
                {
                    Add(item);
                    return false;
                }

                return true;
            }

            return false;
        }

        return false;
    }

    private bool TryTakeItems()
    {
        foreach (var type in _acceptedTypes)
        {
            if (!CanAdd(type))
                continue;

            if (_movingInventory.TryGet(type, out var item))
            {
                Add(item);
                return true;
            }
        }

        return false;
    }
}