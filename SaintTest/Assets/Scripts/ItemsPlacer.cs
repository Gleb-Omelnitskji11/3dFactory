using System.Collections.Generic;
using UnityEngine;

public abstract class ItemsPlacer : MonoBehaviour
{
    [SerializeField] protected Transform _itemsRoot;
    [SerializeField] protected List<ResourceView> Items = new List<ResourceView>();

    public void Init(PlayerInventory playerInventory)
    {
        playerInventory.OnItemRemoved += OnItemRemoved;
        playerInventory.OnItemAdded += OnItemAdded;
    }

    public virtual void OnItemRemoved(ResourceView item)
    {
        Items.Remove(item);
        item.gameObject.SetActive(false);
    }

    public virtual void OnItemAdded(ResourceView item)
    {
        Items.Add(item);
        item.gameObject.SetActive(true);
    }

    protected abstract void UpdatePositions();
}