using Unity.Entities;
using Unity.Physics;
using UnityEngine.Assertions;

namespace BirdLib.Ecs.Physics
{
    /// <summary>   A collector which exits the query as soon as any hit is detected. </summary>
    ///
    /// <typeparam name="T">    Generic type parameter. </typeparam>
    public struct IgnoreSelfFirstHitCollector<T> : ICollector<T> where T : struct, IQueryResult
    {
        /// <summary>   Gets a value indicating whether the early out on first hit. </summary>
        ///
        /// <value> True. </value>
        public bool EarlyOutOnFirstHit => false;

        /// <summary>   Gets the maximum fraction. </summary>
        ///
        /// <value> The maximum fraction. </value>
        public float MaxFraction { get; private set; }
        /// <summary>   Gets  the number of hits. </summary>
        ///
        /// <value> The total number of hits (0 or 1). </value>
        public int NumHits { get; private set; }
        
        private Entity m_EntityToIgnore;
        private bool m_IgnoreTriggers;
        private T m_ClosestHit;

        /// <summary>   Gets the closest hit. </summary>
        ///
        /// <value> The closest hit. </value>
        public T ClosestHit => m_ClosestHit;
        
        /// <summary>   Constructor. </summary>
        ///
        /// <param name="maxFraction">  The maximum fraction. </param>
        public IgnoreSelfFirstHitCollector(float maxFraction, bool ignoreTriggers, Entity entityToIgnore)
        {
            MaxFraction = maxFraction;
            m_ClosestHit = default(T);
            m_IgnoreTriggers = ignoreTriggers;
            m_EntityToIgnore = entityToIgnore;
            NumHits = 0;
        }

        #region ICollector

        /// <summary>   Adds a hit. </summary>
        ///
        /// <param name="hit">  The hit. </param>
        ///
        /// <returns>   True. </returns>
        public bool AddHit(T hit)
        {
            Assert.IsTrue(hit.Fraction <= MaxFraction);
            if ((m_IgnoreTriggers && (hit.Material.CollisionResponse == CollisionResponsePolicy.RaiseTriggerEvents)) || (hit.Entity == m_EntityToIgnore))
            {
                return false;
            }
            MaxFraction = hit.Fraction;
            m_ClosestHit = hit;
            NumHits = 1;
            return true;
        }

        #endregion
    }
}