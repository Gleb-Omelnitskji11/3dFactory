using System;
using UnityEngine;

public class ResourceView : MonoBehaviour
{
    public ResourceType Type;
    public int Amount;
}

[Serializable]
public class ResourceModel
{
    public ResourceType Type;
    public int Amount = 1;
}