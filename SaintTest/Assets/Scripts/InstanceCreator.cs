using System.Collections.Generic;
using UnityEngine;

public class InstanceCreator : MonoBehaviour
{
    [SerializeField] private ResourceView _resourceView1;
    [SerializeField] private ResourceView _resourceView2;
    [SerializeField] private ResourceView _resourceView3;

    private readonly Dictionary<ResourceType, ResourceView> _resourceViews = new();
    private readonly Dictionary<ResourceType, int> _typeCounts = new();
    public static InstanceCreator Instance;

    private void Awake()
    {
        Instance = this;
        _resourceViews.Add(ResourceType.N1, _resourceView1);
        _resourceViews.Add(ResourceType.N2, _resourceView2);
        _resourceViews.Add(ResourceType.N3, _resourceView3);
        _typeCounts.Add(ResourceType.N1, 0);
        _typeCounts.Add(ResourceType.N2, 0);
        _typeCounts.Add(ResourceType.N3, 0);
    }

    public ResourceView CreateObject(ResourceType resourceType)
    {
        var prefab = _resourceViews[resourceType];
        var item = Instantiate(prefab);
        item.name = $"{prefab.name}_{++_typeCounts[resourceType]}";
        return item;
    }
}