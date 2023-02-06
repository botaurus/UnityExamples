using Unity.Entities;
using Unity.Mathematics;

namespace Game.Tweens.Components
{
    public struct TweenPosition : IComponentData
    {
        public int Ease;
        public float Duration;
        public float3 Target;
        public float3 _start;
        public float _t;
    }
}