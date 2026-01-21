using UnityEngine;

public class StackInventory : ItemsPlacer
{
    [SerializeField] private float _deltaDepth = 0.1f;

    protected override void UpdatePositions()
    {
        for (int i = 0; i < Items.Count; i++)
        {
            float depthY = i > 0 ? Items[i - 1].transform.localScale.y : 0f;
            Items[i].transform.localPosition = new Vector3(
                0f,
                i * (depthY + _deltaDepth),
                0f
            );

            Items[i].transform.localRotation = Quaternion.identity;
        }
    }
}