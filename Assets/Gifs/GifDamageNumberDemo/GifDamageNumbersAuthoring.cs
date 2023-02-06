#if UNITY_EDITOR
using BirdLib.Ecs;
using BirdLib.Ecs.MaterialOverrides;
using Game.DamageNumberGraphics;
using Game.Dev;
using Game.Tweens;
using Game.Tweens.Components;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Gifs.GifDamageNumberDemo
{
    public struct GifDamageNumbersSingleton : IComponentData
    {
        public float Every;
        public float rad;
        public int PerPress;
        public Entity Prefab;
        public bool InputKeyCodeT;
        public float DurMin;
        public float DurMax;
        public float Speed;
        public float gravMin;
        public float gravMax;
        public float drag;
        public float angle;
        public float initForceMin;
        public float initForceMax;
        public float4 ColorA;
        public float4 ColorB;
        public float4 ColorC;
        public float4 ColorD;
        public float4 ColorE;
        public float ColorChangeSpeed;
    }

    public class GifDamageNumbersAuthoring : MonoBehaviour
    {
        public float Every;
        public float rad;
        public int PerPress;
        public GameObject Prefab;
        public float DurMin =1;
        public float DurMax =2;
        public float Speed=1;
        public float gravMin=-1;
        public float gravMax=-2;
        public float drag = 0.01f;
        public float angle = 45;
        public float initForceMin = 1;
        public float initForceMax = 2;
        public Color ColorA = new Color(1,1,1,1);
        public Color ColorB = new Color(1,0,1,1);
        public Color ColorC = new Color(0,0,1,1);
        public Color ColorD = new Color(1,0,0,1);
        public Color ColorE = new Color(1,1,1,1);
        public float ColorChangeSpeed = 0.1f;
        
    }

    public class GifDamageNumbersSingletonBaker : Baker<GifDamageNumbersAuthoring>
    {
        public override void Bake(GifDamageNumbersAuthoring authoring)
        {
            AddComponent(new GifDamageNumbersSingleton()
            {
                rad = authoring.rad,
                Every = authoring.Every,
                PerPress = authoring.PerPress,
                Prefab = GetEntity(authoring.Prefab),
                DurMin = authoring.DurMin,
                DurMax = authoring.DurMax,
                Speed = authoring.Speed,
                gravMin = authoring.gravMin,
                gravMax = authoring.gravMax,
                drag = authoring.drag,
                angle = authoring.angle,
                initForceMin = authoring.initForceMin,
                initForceMax = authoring.initForceMax,
                ColorA = MathUtilsEcs.ColorToFloat4(authoring.ColorA),
                ColorB = MathUtilsEcs.ColorToFloat4(authoring.ColorB),
                ColorC = MathUtilsEcs.ColorToFloat4(authoring.ColorC),
                ColorD = MathUtilsEcs.ColorToFloat4(authoring.ColorD),
                ColorE = MathUtilsEcs.ColorToFloat4(authoring.ColorE),
                ColorChangeSpeed = authoring.ColorChangeSpeed,
            });
        }
    }

    // this can't burst compile, but it can use managed code (monobehaviors, Input, etc
    [UpdateInGroup(typeof(DevInitializationSystemGroup))]
    public partial class GifDamageNumbersSingletonManagedSystem : SystemBase
    {
        protected override void OnCreate()
        {
            RequireForUpdate<GifDamageNumbersSingleton>();
        }

        protected override void OnUpdate()
        {
            var singleton = SystemAPI.GetSingletonRW<GifDamageNumbersSingleton>();
            singleton.ValueRW.InputKeyCodeT = false;
            if (Input.GetKey(KeyCode.T))
            {
                singleton.ValueRW.InputKeyCodeT = true;
            }
        }
    }

    [UpdateInGroup(typeof(DevSystemGroup))]
    [BurstCompile]
    public partial struct GifDamageNumbersSystem : ISystem
    {
        private Random _rand;
        private float _z;
        private float _rad;
        private float _cShift;
        private int _num;
        private float _time;
        
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            _rand = new Random(10);
            _num = 1;
            state.RequireForUpdate<GifDamageNumbersSingleton>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile] 
        public void OnUpdate(ref SystemState state)
        {
            var singleton = SystemAPI.GetSingleton<GifDamageNumbersSingleton>();
            _cShift += SystemAPI.Time.DeltaTime * singleton.ColorChangeSpeed;
            if (_cShift > 5.0f)
            {
                _cShift = 0.0f;
            }

            _time += SystemAPI.Time.DeltaTime;
            if (singleton.InputKeyCodeT)
            {
                if (_time > singleton.Every)
                {
                    _time = 0.0f;
                    _rad += SystemAPI.Time.DeltaTime;
                    var spawnEcb = new EntityCommandBuffer(Allocator.Temp);

                    for (int i = 0; i < singleton.PerPress; i++)
                    {
                        var entity = spawnEcb.Instantiate(singleton.Prefab);
                        spawnEcb.AddComponent<TweenPopcorn2d>(entity, _randomPopcorn(singleton));
                        spawnEcb.SetComponent(entity, new LocalTransform()
                        {
                            Position = _randPosition(),
                            Rotation = _rot(),
                            Scale = 1
                        });
                        spawnEcb.SetComponent(entity, new MaterialOverrideColor()
                        {
                            Color = _color(singleton)
                        });
                        _z -= 0.0001f;

                        spawnEcb.SetComponent(entity, new DamageNumberMatOverride()
                        {
                            _Number = _num
                        });
                        _num += 1;
                        if (_num > 99999)
                            _num = 1;
                    }

                    spawnEcb.Playback(state.EntityManager);
                }

            }


            // reset
            var ecb = new EntityCommandBuffer(Allocator.Temp);
            foreach (var (tween, tweenEvent, color, entity) in
                     SystemAPI.Query<TweenPopcorn2d, TweenEventComplete, MaterialOverrideColor>().WithEntityAccess()
                         .WithNone<TweenMaterialColor>())
            {
                ecb.RemoveComponent<TweenEventComplete>(entity);
                ecb.AddComponent(entity,
                    TweenUtils.CreateTweenMaterialColor(TweenEaseType.InCubic, 0.7f, new float4(color.Color.x, color.Color.y, color.Color.z, 0.5f)));
                ecb.AddComponent(entity, TweenUtils.CreateTweenScale(TweenEaseType.InCubic, 0.7f, 0.0f));
            }

            foreach (var (tween, tweenEvent, tweenColor, entity) in
                     SystemAPI.Query<TweenPopcorn2d, TweenEventComplete, TweenMaterialColor>().WithEntityAccess())
            {
                // ecb.RemoveComponent<TweenEventComplete>(entity);
                // ecb.RemoveComponent<TweenMaterialColor>(entity);
                // ecb.RemoveComponent<TweenScale>(entity);
                //
                // ecb.SetComponent(entity, new MaterialOverrideColor()
                // {
                //     Color = _color(singleton)
                // });
                // ecb.SetComponent(entity, new LocalTransform()
                // {
                //     Position = _randPosition(),
                //     Rotation = _rot(),
                //     Scale = 1
                // });
                // _z -= 0.0001f;
                // ecb.SetComponent(entity, _randomPopcorn(singleton));
                
                ecb.DestroyEntity(entity);
            } 

            ecb.Playback(state.EntityManager);
        }

        private float4 _color(GifDamageNumbersSingleton singleton)
        {
            if(_cShift<1.0f)
                return math.lerp(singleton.ColorA, singleton.ColorB, _cShift);
            if(_cShift < 2.0f)
                return math.lerp(singleton.ColorB, singleton.ColorC, _cShift-1.0f);
            if(_cShift < 3.0f)
                return math.lerp(singleton.ColorC, singleton.ColorD, _cShift-2.0f);
            if(_cShift < 4.0f)
                return math.lerp(singleton.ColorD, singleton.ColorE, _cShift-3.0f);
            return math.lerp(singleton.ColorE, singleton.ColorA, _cShift-4.0f);
        }

        private float3 _randPosition()
        {
            return new float3(_rand.NextFloat(-_rad, _rad), _rand.NextFloat(-_rad, _rad), _z);
        }

        private quaternion _rot()
        {
            return quaternion.identity;
        }

        private TweenPopcorn2d _randomPopcorn(GifDamageNumbersSingleton config)
        {
            return new TweenPopcorn2d()
            {
                Duration = _rand.NextFloat(config.DurMin, config.DurMax),
                AngleRange = config.angle,
                Gravity = _rand.NextFloat2(new float2(0, config.gravMin), new float2(0, config.gravMax)),
                InitialForce = _rand.NextFloat(config.initForceMin, config.initForceMax),
                Speed = config.Speed,
                Drag = config.drag,
                _t = -1
            };
        }
    }
}
#endif
