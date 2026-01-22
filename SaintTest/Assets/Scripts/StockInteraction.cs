using System;
using System.Collections;
using UnityEngine;

public class StockInteraction : MonoBehaviour
{
    [SerializeField] private Stock _stock;
    [SerializeField] private bool _isOutput;

    private bool _isPlayerInside;
    private PlayerInventory _playerInventory;
    private Func<bool> _action;

    private void Awake()
    {
        _action = _isOutput ? TryGiveItems: TryTakeItems;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerInventory inventory))
        {
            _playerInventory = inventory;
            _isPlayerInside = true;
            StartCoroutine(MoveItems());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerInventory inventory) &&
            inventory == _playerInventory)
        {
            _isPlayerInside = false;
            _playerInventory = null;
        }
    }

    private IEnumerator MoveItems()
    {
        while (_isPlayerInside)
        {
            _action.Invoke();

            yield return new WaitForSeconds(ResourceMover.ItemMovingDelay);
        }
    }

    private bool TryGiveItems()
    {
        if (!_playerInventory.CanAdd())
        {
            return false;
        }

        foreach (var type in _stock.AcceptedTypes)
        {
            if (!_stock.TryGet(type, out var item))
            {
                continue;
            }

            if (!_playerInventory.TryAdd(item))
            {
                continue;
            }

            return true;
        }

        return false;
    }

    private bool TryTakeItems()
    {
        foreach (var type in _stock.AcceptedTypes)
        {
            if (!_stock.CanAdd(type))
                continue;

            if (_playerInventory.TryGet(type, out var item))
            {
                if (_stock.TryAdd(item))
                {
                    return true;
                }

                _playerInventory.TryAdd(item);
            }
        }

        return false;
    }
}