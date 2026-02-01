using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Resources
{
    [CreateAssetMenu(fileName = "ResourceItems", menuName = "ScriptableObjects/ResourceItems", order = 1)]
    public class ResourceConfig : ScriptableObject
    {
        [SerializeField] private List<ResourceView> _items = new List<ResourceView>();

        public ResourceView GetItem(ResourceType type)
        {
            foreach (ResourceView item in _items)
            {
                if (item.Type == type)
                    return item;
            }

            throw new NullReferenceException();
        }
    }
}