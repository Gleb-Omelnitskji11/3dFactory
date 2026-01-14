using System.Collections;
using UnityEngine;

public class ResourceMover : MonoBehaviour
{
    public static IEnumerator Move(Transform item, Vector3 from, Vector3 to, float duration)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            item.position = Vector3.Lerp(from, to, t);
            yield return null;
        }

        item.position = to;
    }
}