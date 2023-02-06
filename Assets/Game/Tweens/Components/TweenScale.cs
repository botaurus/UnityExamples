using Unity.Entities;
using Unity.Mathematics;

namespace Game.Tweens.Components
{
    public struct TweenScale : IComponentData
    {
        public int Ease;
        public float Duration;
        public float Target;
        public float _start;
        public float _t;
    }
}