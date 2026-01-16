using System;
using UnityEngine;

public class ResourceView : MonoBehaviour, IResource
{
    public ResourceType Type;
    public void Destroy()
    {
        Destroy(gameObject);
    }
}

[Serializable]
public struct ResourceModel
{
    public ResourceType Type;
    public int Amount;

    public ResourceModel(ResourceType type, int amount = 1)
    {
        Type = type;
        Amount = amount;
    }
}

public interface IResource
{
    void Destroy();
}