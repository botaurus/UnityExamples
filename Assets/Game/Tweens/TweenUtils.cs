using Game.Tweens.Components;
using Unity.Mathematics;

namespace Game.Tweens
{
    public static class TweenUtils
    {
        public static TweenScale CreateTweenScale(TweenEaseType ease, float duration, float target)
        {
            return new TweenScale()
            {
                Ease = (int)ease,
                Duration = duration,
                Target = target,
                _t = -1
            };
        }
        public static TweenMaterialColor CreateTweenMaterialColor(
            TweenEaseType ease, float duration, float4 target)
        {
            return new TweenMaterialColor()
            {
                Ease = (int)ease,
                Duration = duration,
                Target = target,
                _t = -1
            };
        }
    }
}