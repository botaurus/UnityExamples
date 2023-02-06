using BirdLib.Ecs;
using BirdLib.Ecs.MaterialOverrides;
using Game.DamageNumberGraphics;
using Game.Tweens.Components;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Game.Tweens
{
    [BurstCompile]
    [UpdateInGroup(typeof(LateSimulationSystemGroup))]
    public partial struct TweenSystemLate : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            //try to reduce sync points by using the built in entitycommandbuffers 
            var ecbSystem = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>();

            // dependencies are handled automatically in ISystem
            // so the EntityCommandBufferSystem does not need any
            // explicit dependency related calls in this context

            // should I create a separate command buffer for each job?
            // in general, it is usually much safer to make a separate command
            // buffer for each job. Performance cost is negligible.

            float deltaTime = SystemAPI.Time.DeltaTime;

            var ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);
            new TweenPositionJob()
            {
                DeltaTime = deltaTime,
                commands = ecb.AsParallelWriter()
            }.ScheduleParallel();

            ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);
            new TweenScaleJob()
            {
                DeltaTime = deltaTime,
                commands = ecb.AsParallelWriter()
            }.ScheduleParallel();
            
            ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);
            uint offset = (uint)((SystemAPI.Time.ElapsedTime + 1.0f) * 10f);

            new TweenPopcorn2dJob()
            {
                randomSeedOffset = offset,
                DeltaTime = deltaTime,
                commands = ecb.AsParallelWriter()
            }.ScheduleParallel();


            ecb = ecbSystem.CreateCommandBuffer(state.WorldUnmanaged);
            new TweenMaterialColorJob()
            {
                DeltaTime = deltaTime,
                commands = ecb.AsParallelWriter()
            }.ScheduleParallel();

            // the rule with IJobEntity is that if you pass in a
            // jobhandle dependency when you schedule, you need to take care of setting
            // the Dependency property on the systemstate. So in this case, you would do
            // state.Dependency = tweenTranslateJobHandle; at the end.
            //
            // Alternately, if you don't pass in any dependencies when scheduling IJobEntity's,
            // they will automatically get combined with state.Dependency (or Dependency on SystemBase),
            // which would also work here since you have a single chain of jobs.
        }
    }


    [BurstCompile]
    public partial struct TweenPositionJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter commands;

        // use [ChunkIndexInQuery] instead of [EntityIndexInQuery] as you only need unique indices per chunk for ECB to work correctly
        // chunk indices are significantly cheaper for Unity to calculate
        [BurstCompile]
        void Execute([ChunkIndexInQuery]int chunkIdx, Entity entity, RefRW<TweenPosition> tween, TransformAspect trans)
        {
            if (tween.ValueRO._t < -0.000001f)
            {
                tween.ValueRW._t = 0.0f;
                tween.ValueRW._start = trans.LocalPosition;
            }

            if (tween.ValueRO._t < tween.ValueRO.Duration)
            {
                tween.ValueRW._t += DeltaTime;
                if (tween.ValueRO._t >= tween.ValueRO.Duration)
                {
                    commands.AddComponent(chunkIdx, entity, new TweenEventComplete());
                    tween.ValueRW._t = tween.ValueRO.Duration;
                }

                float normT = tween.ValueRO._t / tween.ValueRO.Duration;
                trans.LocalPosition = math.lerp(tween.ValueRO._start, tween.ValueRO.Target,
                    TweenFunctions.Ease(normT, tween.ValueRO.Ease));
            }
        }
    }
    
    [BurstCompile]
    public partial struct TweenScaleJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter commands;

        // use [ChunkIndexInQuery] instead of [EntityIndexInQuery] as you only need unique indices per chunk for ECB to work correctly
        // chunk indices are significantly cheaper for Unity to calculate
        [BurstCompile]
        void Execute([ChunkIndexInQuery]int chunkIdx, Entity entity, RefRW<TweenScale> tween, TransformAspect trans)
        {
            if (tween.ValueRO._t < -0.000001f)
            {
                tween.ValueRW._t = 0.0f;
                tween.ValueRW._start = trans.LocalScale;
            }

            if (tween.ValueRO._t < tween.ValueRO.Duration)
            {
                tween.ValueRW._t += DeltaTime;
                if (tween.ValueRO._t >= tween.ValueRO.Duration)
                {
                    commands.AddComponent(chunkIdx, entity, new TweenEventComplete());
                    tween.ValueRW._t = tween.ValueRO.Duration;
                }

                float normT = tween.ValueRO._t / tween.ValueRO.Duration;
                trans.LocalScale = math.lerp(tween.ValueRO._start, tween.ValueRO.Target,
                    TweenFunctions.Ease(normT, tween.ValueRO.Ease));
            }
        }
    }

    [BurstCompile]
    public partial struct TweenPopcorn2dJob : IJobEntity
    {
        public uint randomSeedOffset;
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter commands;

        // use [ChunkIndexInQuery] instead of [EntityIndexInQuery] as you only need unique indices per chunk for ECB to work correctly
        // chunk indices are significantly cheaper for Unity to calculate
        [BurstCompile]
        void Execute([ChunkIndexInQuery]int chunkIdx, Entity entity, RefRW<TweenPopcorn2d> tween,
            TransformAspect trans)
        {
            if (tween.ValueRO._t < -0.000001f)
            {
                var seed = (uint)randomSeedOffset + (uint)(entity.Index);
                var random = Random.CreateFromIndex(seed);

                tween.ValueRW._t = 0.0f;
                tween.ValueRW._velocity =
                    MathUtilsEcs.Rotate(new float2(0, 1),
                        random.NextFloat(-tween.ValueRO.AngleRange, tween.ValueRO.AngleRange));
                tween.ValueRW._velocity *= tween.ValueRO.InitialForce;
            }

            float delta = DeltaTime;
            bool isOver = tween.ValueRO._t > tween.ValueRO.Duration;
            tween.ValueRW._t += delta;

            delta *= tween.ValueRO.Speed;
            tween.ValueRW._velocity += tween.ValueRO.Gravity * delta;
            float dragMag = math.length(tween.ValueRO._velocity);
            dragMag = dragMag * dragMag * tween.ValueRO.Drag;
            tween.ValueRW._velocity -= tween.ValueRW._velocity * dragMag * delta;
            trans.LocalPosition += MathUtilsEcs.ToFloat3(tween.ValueRW._velocity) * delta;
            if (!isOver)
            {
                if (tween.ValueRO._t >= tween.ValueRO.Duration)
                {
                    commands.AddComponent(chunkIdx, entity, new TweenEventComplete());
                }
            }
        }
    }

    [BurstCompile]
    public partial struct TweenMaterialColorJob : IJobEntity
    {
        public float DeltaTime;
        public EntityCommandBuffer.ParallelWriter commands;

        // use [ChunkIndexInQuery] instead of [EntityIndexInQuery] as you only need unique indices per chunk for ECB to work correctly
        // chunk indices are significantly cheaper for Unity to calculate
        [BurstCompile]
        void Execute([ChunkIndexInQuery]int chunkIdx, Entity entity, RefRW<TweenMaterialColor> tween,
            RefRW<MaterialOverrideColor> color)
        {
            if (tween.ValueRO._t < -0.000001f)
            {
                tween.ValueRW._t = 0.0f;
                tween.ValueRW._start = color.ValueRO.Color;
            }

            if (tween.ValueRO._t < tween.ValueRO.Duration)
            {
                tween.ValueRW._t += DeltaTime;
                if (tween.ValueRO._t >= tween.ValueRO.Duration)
                {
                    commands.AddComponent(chunkIdx, entity, new TweenEventComplete());
                    tween.ValueRW._t = tween.ValueRO.Duration;
                }

                float normT = tween.ValueRO._t / tween.ValueRO.Duration;
                color.ValueRW.Color = math.lerp(tween.ValueRO._start, tween.ValueRO.Target,
                    TweenFunctions.Ease(normT, tween.ValueRO.Ease));
            }
        }
    }
}