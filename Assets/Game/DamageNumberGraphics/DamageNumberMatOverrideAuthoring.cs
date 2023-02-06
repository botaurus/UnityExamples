using Unity.Entities;
using Unity.Rendering;
using UnityEngine;

namespace Game.DamageNumberGraphics
{
    [MaterialProperty("_Number")]
    public struct DamageNumberMatOverride : IComponentData
    {
        public float _Number;
    }

    public class DamageNumberMatOverrideAuthoring : MonoBehaviour
    {
        public int Number;
    }
    public class DamageNumberMatOverrideBaker : Baker<DamageNumberMatOverrideAuthoring>
    {
        public override void Bake(DamageNumberMatOverrideAuthoring authoring)
        {
            AddComponent(new DamageNumberMatOverride()
            {
                _Number = (float)authoring.Number
            });
        }
    }
    
}