using System;
using GameFeelTest.Interfaces;
using GameFeelTest.Utility;
using GameFeelTest.ScriptableObjects;
using UnityEngine;

namespace GameFeelTest.Missile
{
    public class MediumMissile : MissileBase
    {
        private Rigidbody _rigidbody;

        [Header("References")]
        [SerializeField] private MissileSO _missileSO;

        [Header("Camera Shake")]
        [SerializeField] private float _shakeIntensity;
        [SerializeField] private float _shakeDuration;

        private Action<MediumMissile> _poolingAction;
        
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
            if (other.gameObject.CompareTag("MiddleCharacter"))
            {
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
        
        public void InitialiseMissileForPool(Action<MediumMissile> poolingAction)
        {
            _poolingAction = poolingAction;
        }
    }
}
