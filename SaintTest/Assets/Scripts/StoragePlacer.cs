using System.Collections.Generic;
using UnityEngine;

public class StoragePlacer : ItemsPlacer
{
    private const float DeltaDepth = 0.05f;
    [SerializeField] private List<ResourceView> Items;

    protected override void OnItemAdded(ResourceView item)
    {
        Items.Add(item);
        base.OnItemAdded(item);
    }

    protected override void OnItemRemoved(ResourceView item)
    {
        Items.Remove(item);
        base.OnItemRemoved(item);
    }

    protected override void UpdatePositions()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            float depthZ = i > 0 ? Items[i - 1].transform.localScale.z : 0f;
            Items[i].transform.localPosition = new Vector3(
                0f,
                0f,
                -i * (depthZ + DeltaDepth)
            );

            Items[i].transform.localRotation = Quaternion.identity;
        }
    }
}