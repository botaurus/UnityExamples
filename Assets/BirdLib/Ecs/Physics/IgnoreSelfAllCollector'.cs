using Unity.Collections;
using Unity.Entities;
using Unity.Physics;

namespace BirdLib.Ecs.Physics
{
    public struct IgnoreSelfAllCollector<T> : ICollector<T> where T : unmanaged, IQueryResult
    {
        public bool EarlyOutOnFirstHit => _earlyOut;
        private bool _earlyOut;
        public float MaxFraction { get; }
        public int NumHits => idx;
        public NativeArray<T> AllHits;
        private Entity m_EntityToIgnore;
        private bool m_IgnoreTriggers;
        private int idx;
        private int arrayLen;
        public IgnoreSelfAllCollector(float maxDistance, ref NativeArray<T> allHits, bool ignoreTriggers, Entity entityToIgnore)
        {
            arrayLen = allHits.Length;
            _earlyOut = false;
            MaxFraction = maxDistance;
            AllHits = allHits;
            idx = 0;
            m_EntityToIgnore = entityToIgnore;
            m_IgnoreTriggers = ignoreTriggers;
        }
        public bool AddHit(T hit)
        {
            if ((m_IgnoreTriggers && (hit.Material.CollisionResponse == CollisionResponsePolicy.RaiseTriggerEvents)) || (hit.Entity == m_EntityToIgnore))
            {
                return false;
            }

            if (idx >= arrayLen)
            {
                _earlyOut = true;
                return false;
            }

            AllHits[idx] = hit;
            idx++;
            if (idx >= arrayLen)
            {
                _earlyOut = true;
                return false;
            }
            return false;
        }
    }
}