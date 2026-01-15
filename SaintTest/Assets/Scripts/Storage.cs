using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    [SerializeField] private int _capacity = 10;
    [SerializeField] private List<ResourceType> _acceptedTypes;
    [SerializeField] private bool _isOutput;
    [SerializeField] private float _tickInterval = 0.5f;

    private Dictionary<ResourceType, List<ResourceModel>> _itemsByType;

    private bool _isPlayerInside;
    private PlayerInventory _movingInventory;

    private int CapacityPerType => _capacity / _acceptedTypes.Count;

    private void Awake()
    {
        _itemsByType = new Dictionary<ResourceType, List<ResourceModel>>();

        foreach (var type in _acceptedTypes)
            _itemsByType[type] = new List<ResourceModel>();
    }

    public bool CanAdd(ResourceType type)
    {
        return _itemsByType[type].Count < CapacityPerType;
    }
    
    public bool HasItems(List<ResourceModel> ingridients)
    {
        foreach (var item in ingridients)
        {
            if(_itemsByType[item.Type].Count < item.Amount)
                return false;
        }
        return true;
    }

    public bool HasItem(ResourceType type)
    {
        return _itemsByType[type].Count > 0;
    }

    public void Add(ResourceModel item)
    {
        var type = item.Type;
        if (!CanAdd(type))
            return;

        _itemsByType[type].Add(item);
    }

    public ResourceModel Remove(ResourceType type)
    {
        if (!HasItem(type))
            return null;

        var list = _itemsByType[type];
        var item = list[^1];
        list.RemoveAt(list.Count - 1);
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
            bool success = _isOutput ? TryGiveItems() : TryTakeItems();
            // if (!success)
            //     yield break;

            yield return new WaitForSeconds(_tickInterval);
        }
    }

    private bool TryGiveItems()
    {
        foreach (var type in _acceptedTypes)
        {
            if (!HasItem(type))
                continue;

            var item = Remove(type);
            if (!_movingInventory.TryAdd(item))
            {
                Add(item);
                return false;
            }

            return true;
        }

        return false;
    }

    private bool TryTakeItems()
    {
        foreach (var type in _acceptedTypes)
        {
            if (!CanAdd(type))
                continue;

            if (_movingInventory.TryGet(type, out var item))
            {
                Add(item);
                return true;
            }
        }

        return false;
    }
}