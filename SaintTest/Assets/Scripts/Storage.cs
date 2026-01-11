using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public int Capacity;
    
    public int CurrentAmount;
    public ResourceType AcceptedType;

    private readonly Queue<ResourceType> _items = new();

    public bool CanAdd() => _items.Count < Capacity;
    public bool HasItem() => _items.Count > 0;

    public void Add(ResourceType type)
    {
        _items.Enqueue(type);
        CurrentAmount = _items.Count;
    }

    public ResourceType Remove()
    {
        var item = _items.Dequeue();
        CurrentAmount = _items.Count;
        return item;
    }
}