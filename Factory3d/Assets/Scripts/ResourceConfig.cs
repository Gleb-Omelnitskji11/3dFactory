using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceItems", menuName = "ScriptableObjects/ResourceItems", order = 1)]
public class ResourceConfig : ScriptableObject
{
    [SerializeField] private List<ResourceView> _items = new List<ResourceView>();
    
    public ResourceView GetItem(ResourceType type) => _items.Find(item => item.Type == type);
}
