using Unity.Entities;
using UnityEngine;

namespace Game.Dev
{
    public class DevSingletonAuthoring : MonoBehaviour
    { }

    public class DevSingletonBaker : Baker<DevSingletonAuthoring>
    {
        public override void Bake(DevSingletonAuthoring authoring)
        {
            AddComponent(new DevSingleton());
        }
    }
}