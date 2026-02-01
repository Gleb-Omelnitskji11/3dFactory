using Game.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Resources
{
    public class ResourceView : PooledObject
    {
        [FormerlySerializedAs("Type")] public ResourceType _type;

        public ResourceType Type => _type;
    }
}
