using UnityEngine;

namespace GameFeelTest.Utility
{
    public class ParticleUtility : MonoBehaviour
    {
        private ParticleSystem[] allParticleSystems;

        private void Start()
        {
            allParticleSystems = GetComponentsInChildren<ParticleSystem>();
            PlayParticles();
        }

        private void PlayParticles()
        {
            foreach (var particles in allParticleSystems)
            {
                particles.Stop();
                particles.Play();
            }
        }
    }
}
