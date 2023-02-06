// #if UNITY_EDITOR
// using Game.Tweens.Components;
// using Unity.Burst;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Mathematics;
// using Unity.Rendering;
// using Unity.Transforms;
//
// namespace Game.Tweens.Dev
// {
//     [BurstCompile]
//     public partial struct DevTweenSystem : ISystem
//     {
//         private bool isInit;
//         private Random _rand;
//
//         public void OnCreate(ref SystemState state)
//         {
//             _rand = new Random(10);
//         }
//
//         public void OnDestroy(ref SystemState state)
//         {
//         }
//
//         public void OnUpdate(ref SystemState state)
//         {
//             if (!isInit)
//             {
//                 isInit = true;
//                 Entity entityPrefab = Entity.Null;
//                 foreach (var (t, mesh, ent) in
//                          SystemAPI.Query<TransformAspect, RenderMeshArray>()
//                              .WithEntityAccess()
//                              .WithNone<TweenPopcorn2d, TweenTransform>())
//                 {
//                     entityPrefab = ent;
//                     break;
//                 }
//
//                 if (entityPrefab != Entity.Null)
//                 {
//                     var ecbInit = new EntityCommandBuffer(Allocator.Temp);
//
//                     for (int i = 0; i < 1000; i++)
//                     {
//                         var entity = ecbInit.Instantiate(entityPrefab);
//                         var trans = new LocalTransform()
//                         {
//                             Position = new float3(0, 0, 0),
//                             Rotation = quaternion.identity,
//                             Scale = 1
//                         };
//                         ecbInit.SetComponent(entity, trans);
//                         ecbInit.AddComponent<TweenPopcorn2d>(entity);
//                         ecbInit.SetComponent(entity, _randomPopcorn());
//                     }
//
//                     ecbInit.Playback(state.EntityManager);
//                 }
//             }
//
//
//             var ecb = new EntityCommandBuffer(Allocator.Temp);
//             foreach (var (tween, tweenEvent, entity) in
//                      SystemAPI.Query<TweenPopcorn2d, TweenEventComplete>().WithEntityAccess())
//             {
//                 ecb.RemoveComponent<TweenEventComplete>(entity);
//                 ecb.SetComponent(entity, new LocalTransform()
//                 {
//                     Position = new float3(0, 0, 0),
//                     Rotation = quaternion.identity,
//                     Scale = 1
//                 });
//                 ecb.SetComponent(entity, _randomPopcorn());
//             }
//             ecb.Playback(state.EntityManager);
//         }
//
//
//         private TweenPopcorn2d _randomPopcorn()
//         {
//             return new TweenPopcorn2d()
//             {
//                 Duration = _rand.NextFloat(1,5),
//                 AngleRange = 45,
//                 Gravity = _rand.NextFloat2(new float2(0, -3), new float2(0, -10)),
//                 InitialForce = _rand.NextFloat(1, 5),
//                 Speed = _rand.NextFloat(1, 2),
//                 _t = -1
//             };
//         }
//     }
// }
// #endif