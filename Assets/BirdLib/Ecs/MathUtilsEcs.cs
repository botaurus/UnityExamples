using Unity.Mathematics;
using UnityEngine;

namespace BirdLib.Ecs
{
    public static class MathUtilsEcs
    {
        public static float4 ColorToFloat4(Color color)
        {
            return new float4(color.r, color.g, color.b, color.a);
        }
        public static float3 ToFloat3(float2 val)
        {
            return new float3(val.x, val.y, 0);
        }

        public static float2 ToFloat2(float3 val)
        {
            return new float2(val.x, val.y);
        }
        
        public static quaternion RotationAlongVector2Smooth(float2 dir, float offset = 0)
        {
            var dirn = math.normalize(dir);
            float angle = math.degrees(math.atan2(dirn.y, dirn.x));
            return quaternion.AxisAngle(
                new float3(0,0,1),
                angle + offset
            );
        }
        public static float2 Rotate(float2 vec, float degrees)
        {
            float sin = math.sin(math.radians(degrees));
            float cos = math.cos(math.radians(degrees));

            float tx = vec.x;
            float ty = vec.y;
            return new float2((cos * tx) - (sin * ty), (sin * tx) + (cos * ty));
        }
        
        /// <summary>
        /// for 2d, use this on a transform.rotation 
        /// </summary>
        /// <param name="dir"></param>
        /// <param name="offset">in radians</param>
        /// <returns></returns>
        public static quaternion Rotate2dAlongVector2(float2 dir, float offset = 0)
        {
            var dirn = math.normalizesafe(dir);
            return quaternion.AxisAngle(
                new float3(0,0,1),
                math.atan2(dirn.y, dirn.x)+offset
            );
        }
    }
}