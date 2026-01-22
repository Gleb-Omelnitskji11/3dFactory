using System;
using UnityEngine;

[Serializable]
public class ResourceModel
{
    [SerializeField] private ResourceType _type;
    [SerializeField] private int _amount;

    public ResourceModel(ResourceType type, int amount = 1)
    {
        _type = type;
        _amount = amount;
    }

    public ResourceType Type => _type;
    public int Amount => _amount;
}