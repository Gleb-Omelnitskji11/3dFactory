using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StorageBase : MonoBehaviour
{
    [SerializeField] protected ItemsPlacer _itemsPlacer;

    protected virtual void Awake()
    {
        _itemsPlacer.Init(this);
    }
    
    public abstract bool CanAdd(ResourceType type);
    public abstract bool HasItems(List<ResourceModel> ingredients);
    public abstract bool HasItem(ResourceModel ingridient);
    public abstract bool TryAdd(ResourceView item);

    public bool TryGet(ResourceModel itemModel, out List<ResourceView> items)
    {
        bool success = TryGetWithoutRemoving(itemModel, out items);
        if(success) RemoveItems(items);
        return success;
    }
    public abstract bool TryGetWithoutRemoving(ResourceModel item, out List<ResourceView> items);
    public abstract bool TryGet(ResourceType item, out ResourceView items);

    public void RemoveItems(List<ResourceView> items)
    {
        foreach (var item in items)
        {
            RemoveItem(item);
        }
    }

    protected virtual void RemoveItem(ResourceView item)
    {
        OnItemRemoved?.Invoke(item);
    }

    protected virtual void AddItem(ResourceView item)
    {
        OnItemAdded?.Invoke(item);
    }

    public event Action<ResourceView> OnItemRemoved;
    public event Action<ResourceView> OnItemAdded;
}