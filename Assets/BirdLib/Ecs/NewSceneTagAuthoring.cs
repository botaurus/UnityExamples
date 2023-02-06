using Unity.Entities;
using UnityEngine;

namespace BirdLib.Ecs
{
    public class NewSceneTagAuthoring : MonoBehaviour { }

    public class NewSceneTagAuthoringBaker : Baker<NewSceneTagAuthoring>
    {
        public override void Bake(NewSceneTagAuthoring authoring)
        {
            AddComponent(new NewSceneTag());
        }
    }
}