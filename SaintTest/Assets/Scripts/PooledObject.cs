using UnityEngine;

public abstract class PooledObject : MonoBehaviour
{
    public virtual void TurnOff()
    {
        gameObject.SetActive(false);
        if (InstanceCreator.Instance != null)
        {
            InstanceCreator.Instance.ReturnToPool(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}