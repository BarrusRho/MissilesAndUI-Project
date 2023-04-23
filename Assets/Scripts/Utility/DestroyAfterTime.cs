using UnityEngine;

namespace GameFeelTest.Utility
{
    public class DestroyAfterTime : MonoBehaviour
    {
        [SerializeField] private float _destroyTime;

        private void Start()
        {
            Destroy();
        }

        private void Destroy()
        {
            Destroy(this.gameObject, _destroyTime);
        }
    }
}
