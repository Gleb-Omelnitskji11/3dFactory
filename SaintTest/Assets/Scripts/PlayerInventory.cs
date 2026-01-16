using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private int _maxCapacity;
    [SerializeField] private List<ResourceView> _items = new();
    [SerializeField] private ItemsPlacer _itemsPlacer;

    public bool CanAdd() => _items.Count < _maxCapacity;
    public bool CanGet() => _items.Count > 0;

    private void Awake()
    {
        _itemsPlacer.Init(this);
    }

    public bool TryAdd(ResourceView item)
    {
        if (!CanAdd())
            return false;

        if (_items.Contains(item))
            return false;

        AddItem(item);
        return true;
    }

    public bool TryGet(ResourceType type, out ResourceView item)
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

    private void RemoveItem(ResourceView item)
    {
        if (_items.Remove(item))
        {
            OnItemRemoved?.Invoke(item);
        }
    }

    private void AddItem(ResourceView item)
    {
        _items.Add(item);
        OnItemAdded?.Invoke(item);
    }
    
    public event Action<ResourceView> OnItemRemoved;
    public event Action<ResourceView> OnItemAdded;
}