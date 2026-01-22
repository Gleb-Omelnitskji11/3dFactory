using System.Collections.Generic;
using UnityEngine;

public abstract class ItemsPlacer : MonoBehaviour
{
    [SerializeField] protected Transform _itemsRoot;

    protected readonly List<ResourceView> Items = new();

    public virtual void Init(StorageBase storage)
    {
        storage.OnItemRemoved += OnItemRemoved;
        storage.OnItemAdded += OnItemAdded;
    }

    protected virtual void OnItemRemoved(ResourceView item)
    {
        Items.Remove(item);
        item.gameObject.SetActive(false);
        item.transform.SetParent(null, true);
        UpdatePositions();
    }

    protected virtual void OnItemAdded(ResourceView item)
    {
        Items.Add(item);
        item.transform.SetParent(_itemsRoot);
        var oldPos = item.transform.localPosition;
        UpdatePositions();
        MoveResource(item, oldPos);
    }

    protected void MoveResource(ResourceView resource, Vector3 oldPos)
    {
        var newPos = resource.transform.localPosition;
        resource.transform.localPosition = oldPos;
        resource.gameObject.SetActive(true);
        ResourceMover.Move(resource.transform, oldPos, newPos);
    }

    protected abstract void UpdatePositions();
}