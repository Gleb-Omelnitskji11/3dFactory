using System;
using System.Collections.Generic;
using System.Linq;
using Game.Resources;
using Game.Storage;
using UnityEngine;

namespace Game.Player
{
    public class PlayerInventory : StorageBase
    {
        [SerializeField] private int _maxCapacity;
        [SerializeField] private List<ResourceView> _items = new();


        public override bool CanAdd(ResourceType type = ResourceType.N1)
        {
            return _items.Count < _maxCapacity;
        }

        public override bool HasItems(List<ResourceModel> ingredients)
        {
            foreach (var item in ingredients)
            {
                if (_items.Count(x => x._type == item.Type) < item.Amount)
                    return false;
            }

            return true;
        }

        public override bool HasItem(ResourceModel ingredient)
        {
            return _items.Count(x => x._type == ingredient.Type) >= ingredient.Amount;
        }

        public override bool TryAdd(ResourceView item)
        {
            if (!CanAdd())
                return false;

            if (_items.Contains(item))
                return false;

            AddItem(item);
            return true;
        }

        public void AddWithoutChecking(ResourceView item)
        {
            AddItem(item);
        }

        public override bool TryGet(ResourceType type, out ResourceView item)
        {
            int lastIndex = _items.FindLastIndex(x => x._type == type);
            if (lastIndex == -1)
            {
                item = null;
                return false;
            }

            item = _items[lastIndex];
            RemoveItem(item);

            return true;
        }

        protected override void RemoveItem(ResourceView item)
        {
            if (_items.Remove(item))
            {
                base.RemoveItem(item);
            }
        }

        protected override void AddItem(ResourceView item)
        {
            _items.Add(item);
            base.AddItem(item);
        }

        protected override List<ResourceView> GetItems(ResourceModel itemModel)
        {
            var result = new List<ResourceView>();

            int need = itemModel.Amount;

            for (int i = _items.Count - 1; i >= 0 && need > 0; i--)
            {
                if (_items[i]._type != itemModel.Type)
                    continue;

                result.Add(_items[i]);
                need--;
            }

            return result;
        }
    }
}
