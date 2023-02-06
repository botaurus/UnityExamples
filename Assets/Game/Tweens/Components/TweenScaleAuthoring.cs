using System;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Tweens.Components
{
    public class TweenScaleAuthoring : MonoBehaviour
    {
        public TweenEaseType Ease;
        public float Duration;
        public float Target;
    }
    
    public class TweenScaleBaker : Baker<TweenScaleAuthoring>
    {
        public override void Bake(TweenScaleAuthoring authoring)
        {
            AddComponent(new TweenScale()
            {
                Duration = authoring.Duration,
                _t = -1f,
                Target = authoring.Target,
                Ease = (int)authoring.Ease,
            });
        }
    }
}