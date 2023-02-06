using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Game.Tweens.Components
{
    public class TweenPopcorn2dAuthoring : MonoBehaviour
    {
        public float Duration = 2;
        public float AngleRange = 45;
        public float InitialForce = 3;
        public float Drag;
        public float Speed = 2;
        public float2 Gravity = new float2(0,-10);
        
    }

    public class TweenPopcorn2dBaker : Baker<TweenPopcorn2dAuthoring>
    {
        public override void Bake(TweenPopcorn2dAuthoring authoring)
        {
            AddComponent(new TweenPopcorn2d()
            {
                Duration = authoring.Duration,
                AngleRange = authoring.AngleRange,
                InitialForce = authoring.InitialForce,
                Gravity = authoring.Gravity,
                Speed = authoring.Speed,
                Drag = authoring.Drag,
                _t = -1
            });
        }
    }
}