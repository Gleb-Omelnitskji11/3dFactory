using System;
using System.Collections.Generic;
using UnityEngine;

public class StackInventory : ItemsPlacer
{
    //private const float StartZ = -0.9f;
    //private const float ItemDepth = 0.5f;
    private const float DeltaDepth = 0.1f;

    public override void OnItemAdded(ResourceView item)
    {
        base.OnItemAdded(item);
        item.transform.SetParent(_itemsRoot);
        UpdatePositions();
    }

    public override void OnItemRemoved(ResourceView item)
    {
        base.OnItemRemoved(item);
        item.transform.SetParent(null, true);
        UpdatePositions();
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