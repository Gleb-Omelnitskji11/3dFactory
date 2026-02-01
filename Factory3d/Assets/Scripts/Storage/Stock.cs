using System.Collections.Generic;
using Game.Resources;
using UnityEngine;

namespace Game.Storage
{
    public class Stock : StorageBase
    {
        [SerializeField] private int _capacity = 10;
        [SerializeField] private List<ResourceType> _acceptedTypes;

        private readonly SortedDictionary<ResourceType, List<ResourceView>> _itemsByType = new();
        private readonly Dictionary<ResourceType, int> _capacityPerType = new();

        public IReadOnlyList<ResourceType> AcceptedTypes => _acceptedTypes;

        protected override void Awake()
        {
            base.Awake();
            foreach (var type in _acceptedTypes)
            {
                _itemsByType[type] = new List<ResourceView>();
                _capacityPerType[type] = _capacity / _acceptedTypes.Count;
            }
        }

        public override bool CanAdd(ResourceType type)
        {
            if (!_itemsByType.TryGetValue(type, out var value))
            {
                return false;
            }

            return value.Count < _capacityPerType[type];
        }

        public override bool HasItems(List<ResourceModel> ingredients)
        {
            foreach (var item in ingredients)
            {
                if (!_itemsByType.TryGetValue(item.Type, out var itemList) || itemList.Count < item.Amount)
                    return false;
            }

            return true;
        }

        public override bool HasItem(ResourceModel ingredient)
        {
            return _itemsByType.TryGetValue(ingredient.Type, out var itemList) && itemList.Count >= ingredient.Amount;
        }

        public override bool TryAdd(ResourceView item)
        {
            var type = item._type;
            if (!CanAdd(type))
                return false;

            AddItem(item);
            return true;
        }


        public override bool TryGet(ResourceType itemType, out ResourceView item)
        {
            if (!_itemsByType.TryGetValue(itemType, out var itemsList) || itemsList.Count == 0)
            {
                item = null;
                return false;
            }

            item = itemsList[^1];
            RemoveItem(item);

            return true;
        }

        protected override List<ResourceView> GetItems(ResourceModel itemModel)
        {
            List<ResourceView> items = _itemsByType[itemModel.Type]
                .GetRange(_itemsByType[itemModel.Type].Count - itemModel.Amount, itemModel.Amount);

            return items;
        }

        protected override void RemoveItem(ResourceView item)
        {
            if (_itemsByType[item._type].Remove(item))
                base.RemoveItem(item);
        }

        protected override void AddItem(ResourceView item)
        {
            _itemsByType[item._type].Add(item);
            base.AddItem(item);
        }


        private bool HasItem(ResourceType type)
        {
            return _itemsByType[type].Count > 0;
        }
    }
}
