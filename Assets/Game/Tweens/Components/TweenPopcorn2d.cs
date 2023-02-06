using Unity.Entities;
using Unity.Mathematics;

namespace Game.Tweens.Components
{
    // pops upward in a random direction and falls - ie loot, damage numbers 
    public struct TweenPopcorn2d : IComponentData
    {
        public float Duration;
        public float AngleRange;
        public float InitialForce;
        public float2 Gravity;
        public float Speed;
        public float Drag;
        
        public float _t;
        public float2 _velocity;
    }
}