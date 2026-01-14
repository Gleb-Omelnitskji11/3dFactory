using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public int MaxCapacity;
    [SerializeField] private List<ResourceModel> _items = new();

    public bool CanTake() => _items.Count < MaxCapacity;

    public bool TryAdd(ResourceModel item)
    {
        if (_items.Count < MaxCapacity)
        {
            Debug.Log("Add item to player inventory");
            _items.Add(item);
            return true;
        }

        Debug.LogWarning("Can`t add item to player inventory");
        return false;
    }

    public bool TryGet(ResourceType type, out ResourceModel item)
    {
        int lastIndex = _items.FindLastIndex(x => x.Type == type);
        if (lastIndex == -1)
        {
            item = null;
            return false;
        }
        
        
        item = _items[lastIndex];
        _items.RemoveAt(lastIndex);

        return true;
    }
}