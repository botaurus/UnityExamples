using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Tweens.Components
{
    public class TweenPositionAuthoring : MonoBehaviour
    {
        public TweenEaseType Ease;
        public float Duration;
        public float3 Target;
    }
    
    public class TweenPositionBaker : Baker<TweenPositionAuthoring>
    {
        public override void Bake(TweenPositionAuthoring authoring)
        {
            AddComponent(new TweenPosition()
            {
                Duration = authoring.Duration,
                _t = -1f,
                Target = authoring.Target,
                Ease = (int)authoring.Ease,
            });
        }
    }
}