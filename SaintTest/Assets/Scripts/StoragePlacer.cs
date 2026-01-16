using UnityEngine;

public class StoragePlacer : ItemsPlacer
{
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
        UpdatePositions();
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