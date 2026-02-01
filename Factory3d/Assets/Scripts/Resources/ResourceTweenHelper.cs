using DG.Tweening;
using UnityEngine;

namespace Game.Resources
{
    public static class ResourceTweenHelper
    {
        public const float ItemMovingDelay = 0.5f;

        public static Tween Move(Transform item, Vector3 from, Vector3 to)
        {
            item.localPosition = from;
            return item.DOLocalMove(to, ItemMovingDelay).SetEase(Ease.Linear);
        }
    }
}
