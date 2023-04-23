using System;
using System.Collections;
using UnityEngine;

namespace GameFeelTest.PooledObjects
{
    public class PooledBouncyBall : MonoBehaviour
    {
        private Action<PooledBouncyBall> _poolingAction;
        
        public void InitialisePooledBouncyBallForPool(Action<PooledBouncyBall> poolingAction)
        {
            _poolingAction = poolingAction;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
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
