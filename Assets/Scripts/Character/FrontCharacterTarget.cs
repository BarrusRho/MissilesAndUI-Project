using GameFeelTest.Interfaces;
using UnityEngine;


namespace GameFeelTest.Character
{
    public class FrontCharacterTarget : MonoBehaviour, IExplodable
    {
        public void Explode()
        {
            Destroy(this.gameObject, 3f);
        }
    }
}
