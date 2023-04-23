using System;
using System.Collections;
using UnityEngine;

namespace GameFeelTest.PooledObjects
{
    public class PooledShape : MonoBehaviour
    {
        private Action<PooledShape> _poolingAction;

        public void InitialisePooledShapeForPool(Action<PooledShape> poolingAction)
        {
            _poolingAction = poolingAction;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("MiddlePlatform"))
            {
                StartCoroutine(nameof(ReturnToPoolDelayCoroutine));
            }
        }

        private IEnumerator ReturnToPoolDelayCoroutine()
        {
            const float delayTime = 2.5f;
            yield return new WaitForSeconds(delayTime);
            _poolingAction(this);
        }
    }
}