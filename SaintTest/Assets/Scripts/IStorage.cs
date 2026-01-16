using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StorageBase : MonoBehaviour
{
    public abstract bool CanAdd(ResourceType type);
    public abstract bool HasItems(List<ResourceModel> ingridients);
    public abstract bool HasItem(ResourceModel ingridient);
    public abstract  bool TryAdd(ResourceView item);
    public abstract bool TryGet(ResourceModel item, out List<ResourceView> items);
}

// public class TriggerStorageTransport
// {
//     [SerializeField] private Storage _storage;
//     [SerializeField] private bool _isOutput;
//     [SerializeField] private float _tickInterval = 0.5f;
//     
//     private bool _isPlayerInside;
//     private PlayerInventory _movingInventory;
//     
//     private Action _action;
//
//     private void Awake()
//     {
//         _action = _isOutput ? TryGetItems : TryAddItems();
//     }
//     
//     private bool TryGetItems()
//     {
//         foreach (var type in _acceptedTypes)
//         {
//             if (!HasItem(type))
//                 continue;
//
//             if (!_movingInventory.CanAdd())
//             {
//                 var item = _itemsByType[type][^1];
//                 if (_movingInventory.TryAdd(item))
//                 {
//                     Remove(item);
//                 }
//
//                 Add(item);
//                 return false;
//             }
//
//             return true;
//         }
//
//         return false;
//     }
//
//     private bool TryAddItems()
//     {
//         foreach (var type in _acceptedTypes)
//         {
//             if (!CanAdd(type))
//                 continue;
//
//             if (_movingInventory.TryGet(type, out var item))
//             {
//                 Add(item.ResourceModel);
//                 return true;
//             }
//         }
//
//         return false;
//     }
// }