using UnityEngine;

namespace GameFeelTest.ScriptableObjects
{
    [CreateAssetMenu()]
    public class MissileSO : ScriptableObject
    {
        public float missileSpeed;

        public GameObject missilePrefab;
        public GameObject trailParticlesPrefab;
        public GameObject explosionParticlesPrefab;
    }
}
