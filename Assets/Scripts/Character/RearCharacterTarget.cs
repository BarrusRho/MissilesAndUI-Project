using DG.Tweening;
using GameFeelTest.Interfaces;
using UnityEngine;


namespace GameFeelTest.Character
{
    public class RearCharacterTarget : MonoBehaviour, IExplodable
    {
        private Rigidbody _rigidbody;
        public Rigidbody RigidBody => _rigidbody;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            transform.DOMove(new Vector3(-15f, 4f, 21.5f), 5f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        }

        public void Explode()
        {
            transform.DOKill();
            Destroy(this.gameObject);
        }
    }
}
