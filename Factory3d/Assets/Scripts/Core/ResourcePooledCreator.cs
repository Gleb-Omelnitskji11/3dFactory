using System;
using System.Collections.Generic;
using System.Linq;
using Game.Resources;
using UnityEngine;

namespace Game.Core
{
    public class ResourcePooledCreator : MonoBehaviour, IPooledCreator
    {
        [SerializeField] private int _initialPoolSize = 0;
        [SerializeField] private ResourceViewPrefab[] _resourcePrefabs;

        private readonly Dictionary<ResourceType, Queue<ResourceView>> _pools = new();
        private readonly Dictionary<ResourceType, int> _typeCounts = new();

        public static IPooledCreator Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }

            Instance = this;
            Init();
        }

        public ResourceView GetObject(ResourceType resourceType)
        {
            if (!_pools.TryGetValue(resourceType, out var pool))
            {
                throw new Exception($"Resource type {resourceType} is not registered");
            }

            ResourceView item;
            if (pool.Count > 0)
            {
                // Get from pool
                item = pool.Dequeue();
                item.gameObject.SetActive(true);
                item.transform.SetParent(null);
            }
            else
            {
                // Create new if pool is empty
                item = CreateNewObject(resourceType);
            }

            return item;
        }

        public void ReturnToPool(PooledObject itemObj)
        {
            if (itemObj == null) return;

            var item = itemObj as ResourceView;
            var resourceType = item._type;
            if (!_pools.ContainsKey(resourceType))
            {
                throw new Exception($"Resource type {resourceType} is not present in pool");
            }

            item.transform.SetParent(transform);
            _pools[resourceType].Enqueue(item);
        }

        private void Init()
        {
            InitCollection();
            Prewarm();
        }

        private void InitCollection()
        {
            foreach (ResourceType type in (ResourceType[])Enum.GetValues(typeof(ResourceType)))
            {
                _pools.Add(type, new Queue<ResourceView>());
                _typeCounts.Add(type, 0);

                if (Array.FindIndex(_resourcePrefabs, x => x.Type == type) == -1)
                {
                    throw new Exception($"Resource type {type} is not registered");
                }
            }
        }

        private void Prewarm()
        {
            foreach (var resource in _resourcePrefabs)
            {
                for (int i = 0; i < _initialPoolSize; i++)
                {
                    var obj = CreateNewObject(resource.Type);
                    ReturnToPool(obj);
                }
            }
        }

        private ResourceView CreateNewObject(ResourceType resourceType)
        {
            var prefab = _resourcePrefabs.First(x => x.Type == resourceType).Prefab;
            var item = Instantiate(prefab);
            item.name = $"{prefab.name}_{++_typeCounts[resourceType]}";
            item._type = resourceType;
            item.gameObject.SetActive(false);
            item.Initialize(this);
            return item;
        }

        [System.Serializable]
        public class ResourceViewPrefab
        {
            public ResourceType Type;
            public ResourceView Prefab;
        }
    }
}