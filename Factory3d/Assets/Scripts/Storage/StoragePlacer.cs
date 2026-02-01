using UnityEngine;

namespace Game.Storage
{
    public class StoragePlacer : ItemsPlacer
    {
        [SerializeField] private float _deltaDepth = 0.05f;

        protected override void UpdatePositions()
        {
            for (int i = 0; i < Items.Count; i++)
            {
                float depthZ = i > 0 ? Items[i - 1].transform.localScale.z : 0f;
                Items[i].transform.localPosition = new Vector3(
                    0f,
                    0f,
                    -i * (depthZ + _deltaDepth)
                );

                Items[i].transform.localRotation = Quaternion.identity;
            }
        }
    }
}
