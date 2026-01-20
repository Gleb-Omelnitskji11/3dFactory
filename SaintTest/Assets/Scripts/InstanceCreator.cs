using System.Collections.Generic;
using UnityEngine;

public class InstanceCreator : MonoBehaviour
{
    [SerializeField] private ResourceView _resourceView1;
    [SerializeField] private ResourceView _resourceView2;
    [SerializeField] private ResourceView _resourceView3;
    [SerializeField] private int _initialPoolSize = 0;

    private readonly Dictionary<ResourceType, ResourceView> _prefabs = new();
    private readonly Dictionary<ResourceType, Queue<ResourceView>> _pools = new();
    private readonly Dictionary<ResourceType, int> _typeCounts = new();
    public static InstanceCreator Instance;

    private void Awake()
    {
        if (Instance != null) Destroy(Instance);
        Instance = this;
        Init();
    }

    public ResourceView CreateObject(ResourceType resourceType)
    {
        var pool = _pools[resourceType];

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
        var resourceType = item.Type;
        if (!_pools.ContainsKey(resourceType)) return;

        item.gameObject.SetActive(false);
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
        _prefabs.Add(ResourceType.N1, _resourceView1);
        _prefabs.Add(ResourceType.N2, _resourceView2);
        _prefabs.Add(ResourceType.N3, _resourceView3);

        _pools.Add(ResourceType.N1, new Queue<ResourceView>());
        _pools.Add(ResourceType.N2, new Queue<ResourceView>());
        _pools.Add(ResourceType.N3, new Queue<ResourceView>());

        _typeCounts.Add(ResourceType.N1, 0);
        _typeCounts.Add(ResourceType.N2, 0);
        _typeCounts.Add(ResourceType.N3, 0);
    }

    private void Prewarm()
    {
        foreach (var resourceType in _prefabs.Keys)
        {
            for (int i = 0; i < _initialPoolSize; i++)
            {
                var obj = CreateNewObject(resourceType);
                ReturnToPool(obj);
            }
        }
    }

    private ResourceView CreateNewObject(ResourceType resourceType)
    {
        var prefab = _prefabs[resourceType];
        var item = Instantiate(prefab);
        item.name = $"{prefab.name}_{++_typeCounts[resourceType]}";
        item.Type = resourceType;
        item.gameObject.SetActive(false);
        return item;
    }
}