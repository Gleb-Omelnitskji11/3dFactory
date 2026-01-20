using UnityEngine;

public abstract class PooledObject : MonoBehaviour
{
    public virtual void Destroy()
    {
        gameObject.SetActive(false);
        if (InstanceCreator.Instance != null)
        {
            InstanceCreator.Instance?.ReturnToPool(this);
        }
        else
        {
            UnityEngine.Object.Destroy(gameObject);
        }
    }
}