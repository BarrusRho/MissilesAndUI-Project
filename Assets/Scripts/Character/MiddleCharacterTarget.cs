using GameFeelTest.Interfaces;
using UnityEngine;


namespace GameFeelTest.Character
{
    public class MiddleCharacterTarget : MonoBehaviour, IExplodable
    {
        public void Explode()
        {
            Destroy(this.gameObject);
        }
    }
}
