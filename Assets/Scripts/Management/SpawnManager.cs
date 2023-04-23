using System;
using System.Collections;
using GameFeelTest.Character;
using GameFeelTest.Missile;
using GameFeelTest.PooledObjects;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace GameFeelTest.Management
{
    public class SpawnManager : MonoBehaviour
    {
        private static SpawnManager _instance = null;
        public static SpawnManager Instance => _instance;

        [Header("Missiles")]
        [SerializeField] private SmallMissile _smallMissilePrefab;
        [SerializeField] private Transform _smallMissileSpawnPoint;

        [SerializeField] private MediumMissile _mediumMissilePrefab;
        [SerializeField] private Transform _mediumMissileSpawnPoint;

        [SerializeField] private BigMissile _bigMissilePrefab;
        [SerializeField] private Transform _bigMissileSpawnPoint;

        [Header("Characters")]
        [SerializeField] private FrontCharacterTarget _frontCharacterPrefab;
        [SerializeField] private MiddleCharacterTarget _middleCharacterPrefab;
        [SerializeField] private RearCharacterTarget _rearCharacterPrefab;
        [SerializeField] private Transform _frontCharacterSpawnPoint;
        [SerializeField] private Transform _middleCharacterSpawnPoint;
        [SerializeField] private Transform _rearCharacterSpawnPoint;
        [SerializeField] private float _spawnCharacterTime = 2f;
        
        [Header("Pooled Objects")]
        [SerializeField] private PooledBouncyBall _pooledBouncyBallPrefab;
        [SerializeField] private Transform _pooledBouncyBallSpawnPoint;
        [SerializeField] private int _pooledBouncyBallSpawnAmount = 30;
        [SerializeField] private PooledShape _pooledShapePrefab;
        [SerializeField] private Transform[] _pooledShapeSpawnPoints;

        private ObjectPool<SmallMissile> _smallMissilePool;
        private ObjectPool<MediumMissile> _mediumMissilePool;
        private ObjectPool<BigMissile> _bigMissilePool;
        private ObjectPool<PooledBouncyBall> _pooledBouncyBallPool;
        private ObjectPool<PooledShape> _pooledShapePool;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else if (_instance != this)
            {
                Destroy(this.gameObject, 0f);
            }
        }

        private void OnEnable()
        {
            SmallMissile.OnMissileImpact += SmallMissileOnOnMissileImpact;
            MediumMissile.OnMissileImpact += MediumMissileOnOnMissileImpact;
            BigMissile.OnMissileImpact += BigMissileOnOnMissileImpact;
        }

        private void OnDisable()
        {
            SmallMissile.OnMissileImpact -= SmallMissileOnOnMissileImpact;
            MediumMissile.OnMissileImpact -= MediumMissileOnOnMissileImpact;
            BigMissile.OnMissileImpact -= BigMissileOnOnMissileImpact;
        }

        private void Start()
        {
            CreateSmallMissilePool();
            CreateMediumMissilePool();
            CreateBigMissilePool();
            CreatePooledBouncyBallPool();
            CreatePooledShapePool();
            
            SpawnPooledShapeFromPool();
        }

        #region MissilePools

        private void CreateSmallMissilePool()
        {
            _smallMissilePool = new ObjectPool<SmallMissile>(() => { return Instantiate(_smallMissilePrefab); },
                smallMissile => { smallMissile.gameObject.SetActive(true); },
                smallMissile => { smallMissile.gameObject.SetActive(false); },
                smallMissile => { Destroy(smallMissile.gameObject); }, true, 10, 20);
        }

        public void GetSmallMissileFromPool()
        {
            var smallMissile = _smallMissilePool.Get();
            smallMissile.transform.position = _smallMissileSpawnPoint.transform.position;
            smallMissile.InitialiseMissileForPool(ReturnSmallMissileToPool);
        }

        public void ReturnSmallMissileToPool(SmallMissile smallMissile)
        {
            _smallMissilePool.Release(smallMissile);
        }

        private void CreateMediumMissilePool()
        {
            _mediumMissilePool = new ObjectPool<MediumMissile>(() => { return Instantiate(_mediumMissilePrefab); },
                mediumMissile => { mediumMissile.gameObject.SetActive(true); },
                mediumMissile => { mediumMissile.gameObject.SetActive(false); },
                mediumMissile => { Destroy(mediumMissile.gameObject); }, true, 10, 20);
        }

        public void GetMediumMissileFromPool()
        {
            var mediumMissile = _mediumMissilePool.Get();
            mediumMissile.transform.position = _mediumMissileSpawnPoint.transform.position;
            mediumMissile.GetComponent<MediumMissile>().LaunchMissile();
            mediumMissile.InitialiseMissileForPool(ReturnMediumMissileToPool);
        }

        public void ReturnMediumMissileToPool(MediumMissile mediumMissile)
        {
            _mediumMissilePool.Release(mediumMissile);
        }

        private void CreateBigMissilePool()
        {
            _bigMissilePool = new ObjectPool<BigMissile>(() => { return Instantiate(_bigMissilePrefab); },
                bigMissile => { bigMissile.gameObject.SetActive(true); },
                bigMissile => { bigMissile.gameObject.SetActive(false); },
                bigMissile => { Destroy(bigMissile.gameObject); }, true, 10, 20);
        }

        public void GetBigMissileFromPool()
        {
            var bigMissile = _bigMissilePool.Get();
            bigMissile.transform.position = _bigMissileSpawnPoint.transform.position;
            bigMissile.GetComponent<BigMissile>().LaunchMissile();
            bigMissile.InitialiseMissileForPool(ReturnBigMissileToPool);
        }

        public void ReturnBigMissileToPool(BigMissile bigMissile)
        {
            _bigMissilePool.Release(bigMissile);
        }
        
        #endregion

        #region PooledObjects

        private void CreatePooledBouncyBallPool()
        {
            _pooledBouncyBallPool = new ObjectPool<PooledBouncyBall>(
                () => { return Instantiate(_pooledBouncyBallPrefab); },
                pooledBouncyBall => { pooledBouncyBall.gameObject.SetActive(true); },
                pooledBouncyBall => { pooledBouncyBall.gameObject.SetActive(false); },
                pooledBouncyBall => { Destroy(pooledBouncyBall.gameObject); }, false, 50, 100);
        }

        public void SpawnPooledBouncyBallFromPool()
        {
            for (int i = 0; i < _pooledBouncyBallSpawnAmount; i++)
            {
                var pooledBouncyBall = _pooledBouncyBallPool.Get();
                var sphereRadius = 5f;
                pooledBouncyBall.transform.position = _pooledBouncyBallSpawnPoint.transform.position +
                                                      Random.insideUnitSphere * sphereRadius;
                pooledBouncyBall.InitialisePooledBouncyBallForPool(ReturnPooledBouncyBallToPool);
            }
        }

        public void ReturnPooledBouncyBallToPool(PooledBouncyBall pooledBouncyBall)
        {
            _pooledBouncyBallPool.Release(pooledBouncyBall);
        }
        
        private void CreatePooledShapePool()
        {
            _pooledShapePool = new ObjectPool<PooledShape>(
                () => { return Instantiate(_pooledShapePrefab); },
                pooledShape => { pooledShape.gameObject.SetActive(true); },
                pooledShape => { pooledShape.gameObject.SetActive(false); },
                pooledShape => { Destroy(pooledShape.gameObject); }, false, 30, 60);
        }

        public void SpawnPooledShapeFromPool()
        {
            foreach (var pooledShapeTransform in _pooledShapeSpawnPoints)
            {
                var pooledShaped = _pooledShapePool.Get();
                pooledShaped.transform.position = pooledShapeTransform.transform.position;
                pooledShaped.InitialisePooledShapeForPool(ReturnPooledShapeToPool);
            }
        }

        public void ReturnPooledShapeToPool(PooledShape pooledShape)
        {
            _pooledShapePool.Release(pooledShape);
        }

        #endregion

        #region Events

        private void SmallMissileOnOnMissileImpact(object sender, EventArgs eventArgs)
        {
            StartCoroutine(nameof(SpawnRearCharacterRoutine));
        }

        private void MediumMissileOnOnMissileImpact(object sender, EventArgs eventArgs)
        {
            StartCoroutine(nameof(SpawnMiddleCharacterRoutine));
            SpawnPooledBouncyBallFromPool();
        }

        private void BigMissileOnOnMissileImpact(object sender, EventArgs eventArgs)
        {
            StartCoroutine(nameof(SpawnFrontCharacterRoutine));
        }

        #endregion

        #region Coroutines

        private IEnumerator SpawnFrontCharacterRoutine()
        {
            yield return new WaitForSeconds(_spawnCharacterTime);
            Instantiate(_frontCharacterPrefab, _frontCharacterSpawnPoint);
            SpawnPooledShapeFromPool();
        }

        private IEnumerator SpawnMiddleCharacterRoutine()
        {
            yield return new WaitForSeconds(_spawnCharacterTime);
            Instantiate(_middleCharacterPrefab, _middleCharacterSpawnPoint);
        }

        private IEnumerator SpawnRearCharacterRoutine()
        {
            yield return new WaitForSeconds(_spawnCharacterTime);
            Instantiate(_rearCharacterPrefab, _rearCharacterSpawnPoint);
        }

        #endregion
    }
}