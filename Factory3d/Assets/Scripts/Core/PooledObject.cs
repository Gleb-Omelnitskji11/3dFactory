using UnityEngine;

namespace Game.Core
{
    public abstract class PooledObject : MonoBehaviour
    {
        private IPooledCreator _pool;

        public void Initialize(IPooledCreator pool)
        {
            _pool = pool;
        }

        public virtual void TurnOff()
        {
            gameObject.SetActive(false);
            _pool.ReturnToPool(this);
        }
    }
}