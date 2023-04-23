using System;
using GameFeelTest.Interfaces;
using GameFeelTest.Utility;
using GameFeelTest.ScriptableObjects;
using UnityEngine;

namespace GameFeelTest.Missile
{
    public class BigMissile : MissileBase
    {
        private Rigidbody _rigidbody;

        [Header("References")]
        [SerializeField] private MissileSO _missileSO;

        [Header("Explosion Variables")] 
        [SerializeField] private float _explosionRadius = 5f;
        [SerializeField] private float _explosionForce = 500f;

        [Header("Camera Shake")]
        [SerializeField] private float _shakeIntensity;
        [SerializeField] private float _shakeDuration;

        private Action<BigMissile> _poolingAction;
        
        public static event EventHandler OnMissileImpact;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        public override void LaunchMissile()
        {
            _rigidbody.velocity = new Vector3(0, 0,  _missileSO.missileSpeed);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("FrontCharacter"))
            {
                var surroundingObjects = Physics.OverlapSphere(transform.position, _explosionRadius);

                foreach (var surroundingObject in surroundingObjects)
                {
                    var surroundingObjectRigidbody = surroundingObject.GetComponent<Rigidbody>();

                    if (surroundingObjectRigidbody == null)
                    {
                        continue;
                    }
                    
                    surroundingObjectRigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius, 1.2f);
                }
                
                OnMissileImpact?.Invoke(this, EventArgs.Empty);
                CameraShake.Instance.ShakeCamera(_shakeIntensity, _shakeDuration);
                _poolingAction(this);

                Instantiate(_missileSO.explosionParticlesPrefab, transform.position, Quaternion.identity);

                if (other.transform.TryGetComponent<IExplodable>(out var target))
                {
                    target.Explode();
                }
            }
        }
        
        public void InitialiseMissileForPool(Action<BigMissile> poolingAction)
        {
            _poolingAction = poolingAction;
        }
    }
}
