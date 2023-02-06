using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using UnityEngine;

namespace BirdLib.Ecs.MaterialOverrides
{
    [MaterialProperty("_Color")]
    public struct MaterialOverrideColor : IComponentData
    {
        public float4 Color;
    }

    public class MaterialOverrideColorAuthoring : MonoBehaviour
    {
        public float4 Color = new float4(1,1,1,1);
    }
    public class MaterialOverrideColorBaker : Baker<MaterialOverrideColorAuthoring>
    {
        public override void Bake(MaterialOverrideColorAuthoring authoring)
        {
            AddComponent(new MaterialOverrideColor()
            {
                Color = authoring.Color
            });
        }
    }
}