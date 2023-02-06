using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Tweens.Components
{
    public class TweenMaterialColorAuthoring : MonoBehaviour
    {
        public TweenEaseType Ease;
        public float Duration;
        public float4 Target = new float4(1,1,1,0);
    }

    public class TweenMaterialColorBaker : Baker<TweenMaterialColorAuthoring>
    {
        public override void Bake(TweenMaterialColorAuthoring authoring)
        {
            AddComponent(new TweenMaterialColor()
            {
                Ease = (int)authoring.Ease,
                Duration = authoring.Duration,
                Target = authoring.Target,
                _t = -1
            });
        }
    }
}