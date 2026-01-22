using DG.Tweening;
using UnityEngine;

public class ResourceMover : MonoBehaviour
{
    public const float ItemMovingDelay = 0.5f;

    public static Tween Move(Transform item, Vector3 from, Vector3 to)
    {
        item.localPosition = from;
        return item.DOLocalMove(to, ItemMovingDelay).SetEase(Ease.Linear);
    }
}
