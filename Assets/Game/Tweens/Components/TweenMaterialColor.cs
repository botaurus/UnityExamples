using Unity.Entities;
using Unity.Mathematics;

namespace Game.Tweens.Components
{
    public struct TweenMaterialColor : IComponentData
    {
        public int Ease;
        public float Duration;
        public float4 Target;
        public float4 _start;
        public float _t;
    }
}