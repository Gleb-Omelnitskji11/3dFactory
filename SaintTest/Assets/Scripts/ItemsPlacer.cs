using System.Collections.Generic;
using UnityEngine;

public abstract class ItemsPlacer : MonoBehaviour
{
    [SerializeField] protected Transform _itemsRoot;

    public virtual void Init(StorageBase playerInventory)
    {
        playerInventory.OnItemRemoved += OnItemRemoved;
        playerInventory.OnItemAdded += OnItemAdded;
    }

    protected virtual void OnItemRemoved(ResourceView item)
    {
        item.gameObject.SetActive(false);
        item.transform.SetParent(null, true);
        UpdatePositions();
    }

    protected virtual void OnItemAdded(ResourceView item)
    {
        item.gameObject.SetActive(true);
        item.transform.SetParent(_itemsRoot);
        UpdatePositions();
    }

    protected abstract void UpdatePositions();
}