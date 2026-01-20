using System.Collections.Generic;
using UnityEngine;

public class StackInventory : ItemsPlacer
{
    private const float DeltaDepth = 0.1f;
    private readonly StackInventory _stackInventory;

    [SerializeField] private List<ResourceView> Items;
    
    public override void Init(StorageBase playerInventory)
    {
        base.Init(playerInventory);
        Items = playerInventory.Items as List<ResourceView>;
    }


    protected override void UpdatePositions()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            float depthY = i > 0 ? Items[i - 1].transform.localScale.y : 0f;
            Items[i].transform.localPosition = new Vector3(
                0f,
                i * (depthY + DeltaDepth),
                0f
            );

            Items[i].transform.localRotation = Quaternion.identity;
        }
    }
}