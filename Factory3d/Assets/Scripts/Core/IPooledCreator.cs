using Game.Core;
using Game.Resources;

public interface IPooledCreator
{
    public ResourceView GetObject(ResourceType resourceType);
    public void ReturnToPool(PooledObject itemObj);
}