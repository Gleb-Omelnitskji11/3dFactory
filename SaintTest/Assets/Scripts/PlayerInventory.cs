using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int MaxCapacity;
    public int CurrentAmount;
    private readonly List<ResourceModel> _items = new();

    public bool CanTake() => _items.Count < MaxCapacity;

    public bool TryAdd(ResourceModel item)
    {
        if (_items.Count < MaxCapacity)
        {
            Debug.Log("Add item to player inventory");
            _items.Add(item);
            CurrentAmount += item.Amount;
            return true;
        }

        Debug.LogWarning("Can`t add item to player inventory");
        return false;
    }

    public ResourceModel Remove(ResourceType type)
    {
        var item = _items.FindLast(x => x.Type == type);
        _items.RemoveAt(_items.Count - 1);
        CurrentAmount--;
        return item;
    }
}