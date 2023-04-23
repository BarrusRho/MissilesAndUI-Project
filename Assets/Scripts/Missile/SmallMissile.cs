using System;
using GameFeelTest.Character;
using GameFeelTest.Interfaces;
using GameFeelTest.Utility;
using GameFeelTest.ScriptableObjects;
using UnityEngine;

namespace GameFeelTest.Missile
{
    public class SmallMissile : MissileBase
    {
        [Header("References")] private Rigidbody _rigidbody;
        [SerializeField] private MissileSO _missileSO;
        [SerializeField] private RearCharacterTarget _target;
        [SerializeField] private GameObject _trailParticles;
        [SerializeField] private GameObject _explosionParticles;

        [Header("Movement")] [SerializeField] private float _rotateSpeed = 95;
        [SerializeField] private float _maxDistancePredict = 100;
        [SerializeField] private float _minDistancePredict = 5;
        [SerializeField] private float _maxTimePrediction = 5;
        private Vector3 _standardPrediction, _deviatedPrediction;
        [SerializeField] private float _deviationAmount = 50;
        [SerializeField] private float _deviationSpeed = 2;

        [Header("Camera Shake")] [SerializeField]
        private float _shakeIntensity;

        [SerializeField] private float _shakeDuration;

        private Action<SmallMissile> _poolingAction;

        public static event EventHandler OnMissileImpact;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _target = FindObjectOfType<RearCharacterTarget>();
        }

        private void Update()
        {
            _target = FindObjectOfType<RearCharacterTarget>();
        }

        private void FixedUpdate()
        {
            _rigidbody.velocity = transform.forward * _missileSO.missileSpeed;

            var leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict,
                Vector3.Distance(transform.position, _target.transform.position));

            PredictMovement(leadTimePercentage);

            AddDeviation(leadTimePercentage);

            RotateRocket();
        }

        public override void LaunchMissile()
        {
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("RearCharacter"))
            {
                OnMissileImpact?.Invoke(this, EventArgs.Empty);
                CameraShake.Instance.ShakeCamera(_shakeIntensity, _shakeDuration);
                _poolingAction(this);
                Instantiate(_explosionParticles, transform.position, Quaternion.identity);

                if (other.transform.TryGetComponent<IExplodable>(out var target))
                {
                    target.Explode();
                }
            }
        }

        public void InitialiseMissileForPool(Action<SmallMissile> poolingAction)
        {
            _poolingAction = poolingAction;
        }

        #region HomingMissileLogic

        private void PredictMovement(float leadTimePercentage)
        {
            var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);

            _standardPrediction = _target.RigidBody.position + _target.RigidBody.velocity * predictionTime;
        }

        private void AddDeviation(float leadTimePercentage)
        {
            var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);

            var predictionOffset = transform.TransformDirection(deviation) * _deviationAmount * leadTimePercentage;

            _deviatedPrediction = _standardPrediction + predictionOffset;
        }

        private void RotateRocket()
        {
            var heading = _deviatedPrediction - transform.position;

            var rotation = Quaternion.LookRotation(heading);
            _rigidbody.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation,
                _rotateSpeed * Time.deltaTime));
        }

        #endregion
    }
}