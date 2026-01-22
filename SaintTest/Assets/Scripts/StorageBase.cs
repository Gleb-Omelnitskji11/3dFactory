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
    public abstract bool HasItems(List<ResourceModel> ingridients);
    public abstract bool HasItem(ResourceModel ingridient);
    public abstract bool TryAdd(ResourceView item);
    public abstract bool TryGet(ResourceModel item, out List<ResourceView> items);
    public abstract bool TryGet(ResourceType item, out ResourceView items);


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