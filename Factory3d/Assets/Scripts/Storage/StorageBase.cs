using System;
using System.Collections.Generic;
using Game.Resources;
using UnityEngine;

namespace Game.Storage
{
    public abstract class StorageBase : MonoBehaviour
    {
        [SerializeField] protected ItemsPlacer _itemsPlacer;

        protected virtual void Awake()
        {
            _itemsPlacer.Init(this);
        }
        
        public abstract bool CanAdd(ResourceType type);
        public abstract bool HasItems(List<ResourceModel> ingredients);
        public abstract bool HasItem(ResourceModel ingredient);
        public abstract bool TryAdd(ResourceView item);

        public bool TryGet(ResourceModel itemModel, out List<ResourceView> items)
        {
            bool success = TryGetWithoutRemoving(itemModel, out items);
            if(success) RemoveItems(items);
            return success;
        }
        public bool TryGetWithoutRemoving(ResourceModel itemModel, out List<ResourceView> output)
        {
            if (!HasItem(itemModel))
            {
                output = null;
                return false;
            }

            output = GetItems(itemModel);
            return true;
        }
        public abstract bool TryGet(ResourceType itemType, out ResourceView item);

        public void RemoveItems(List<ResourceView> items)
        {
            foreach (var item in items)
            {
                RemoveItem(item);
            }
        }

        protected abstract List<ResourceView> GetItems(ResourceModel itemModel);

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
}
