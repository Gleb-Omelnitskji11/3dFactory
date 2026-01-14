using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public int Capacity;
    public int CurrentAmount;
    public ResourceType AcceptedType;
    [SerializeField] private float _tickInterval = 0.5f;

    private bool _isPlayerInside;
    private PlayerInventory _currentInventory;

    private readonly Queue<ResourceModel> _items = new();

    public bool CanAdd() => _items.Count < Capacity;
    public bool HasItem() => _items.Count > 0;

    public void Add(ResourceType type)
    {
        var item = new ResourceModel() { Amount = 1, Type = AcceptedType };
        _items.Enqueue(item);
        CurrentAmount += item.Amount;
    }

    public ResourceModel Remove()
    {
        var item = _items.Dequeue();
        CurrentAmount -= item.Amount;
        return item;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerInventory inventory))
        {
            Debug.Log("Player come");
            _currentInventory = inventory;
            _isPlayerInside = true;
            StartCoroutine(GiveItemsInPlayerInventory());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerInventory inventory) &&
            inventory == _currentInventory)
        {
            Debug.Log("Player exit");
            _isPlayerInside = false;
            _currentInventory = null;
        }
    }

    private IEnumerator GiveItemsInPlayerInventory()
    {
        while (_isPlayerInside)
        {
            if (CurrentAmount > 0)
            {
                int taken = Mathf.Min(1, CurrentAmount);
                ResourceModel item = Remove();
                bool accepted = _currentInventory.TryAdd(item);

                if (!accepted)
                {
                    Debug.LogError(
                        $"Break CollectRoutine _isPlayerInside{_isPlayerInside}, CurrentAmount {CurrentAmount}");
                    yield break;
                }

                CurrentAmount -= taken;
            }

            yield return new WaitForSeconds(_tickInterval);
        }

        Debug.LogError($"Stop CollectRoutine _isPlayerInside{_isPlayerInside}, CurrentAmount {CurrentAmount}");
    }

    private bool TryTakeItemFromStock(out ResourceModel item)
    {
        if (CurrentAmount <= 0)
        {
            item = null;
            return false;
        }

        item = new ResourceModel() { Amount = 1, Type = AcceptedType };
        return true;
    }
}