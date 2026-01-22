using System.Collections;
using UnityEngine;

public class ResourceMover : MonoBehaviour
{
    public const float ItemMovingDelay = 0.5f;

    public static IEnumerator Move(Transform item, Vector3 from, Vector3 to)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / ItemMovingDelay;
            item.localPosition = Vector3.Lerp(from, to, t);
            yield return null;
        }

        item.localPosition = to;
    }
}