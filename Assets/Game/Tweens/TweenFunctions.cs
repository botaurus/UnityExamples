using System;
using Unity.Burst;
using Unity.Mathematics;

namespace Game.Tweens
{
    [BurstCompile]
    public static class TweenFunctions
    {
        [BurstCompile]
        public static float OutCubic(float t)
        {
            return 1 - math.pow(1 - t, 3);
        }

        [BurstCompile]
        public static float InCubic(float t)
        {
            return t * t * t;
        }

        [BurstCompile]
        public static float OutElastic(float t)
        {
            float p = 0.3f;
            return (float)math.pow(2, -10 * t) * (float)math.sin((t - p / 4) * (2 * math.PI) / p) + 1;
        }

        [BurstCompile]
        public static float Ease(float t, int idx)
        {
            if (idx == 0)
                return t;
            if (idx == 1)
                return OutCubic(t);
            if (idx == 2)
                return InCubic(t);
            if (idx == 3)
                return OutElastic(t);

            throw new ArgumentException($"Ease idx {idx} not supported");
        }
    }
}