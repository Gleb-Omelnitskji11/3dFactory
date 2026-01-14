using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public int Capacity;
    public ResourceType AcceptedType;
    [SerializeField] private bool _isOutput;
    [SerializeField] private float _tickInterval = 0.5f;

    private bool _isPlayerInside;
    private PlayerInventory _movingInventory;

    [SerializeField] private List<ResourceModel> _items = new();

    public bool CanAdd() => _items.Count < Capacity;
    public bool HasItem() => _items.Count > 0;

    public void Add(ResourceModel item)
    {
        if (!CanAdd())
            return;
        _items.Add(item);
    }

    public ResourceModel Remove()
    {
        if (!HasItem())
            return null;
        
        var item = _items[^1];
        _items.RemoveAt(_items.Count - 1);
        return item;
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
            if (_isOutput && !TryGiveItems() || !_isOutput && !TryTakeItems())
            {
                //receiving inventory is full
                yield break;
            }

            yield return new WaitForSeconds(_tickInterval);
        }
    }

    private bool TryGiveItems()
    {
        if (HasItem())
        {
            ResourceModel item = Remove();
            bool accepted = _movingInventory.TryAdd(item);

            if (!accepted)
            {
                return false;
            }
        }

        return true;
    }

    private bool TryTakeItems()
    {
        if (CanAdd())
        {
            bool accepted = _movingInventory.TryGet(AcceptedType, out ResourceModel item);

            if (!accepted)
            {
                return false;
            }

            Add(item);
        }

        return true;
    }
}