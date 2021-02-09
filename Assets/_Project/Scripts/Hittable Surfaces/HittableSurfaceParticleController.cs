using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.HittableSurfaces
{
    public class HittableSurfaceParticleController : MonoBehaviour
    {
        public float reloadParticleTime = 2f;
        public List<ParticleSystem> particleSystems = new List<ParticleSystem>();

        private HittableSurfaceParticlePoolingItemController particlePoolingController;
        public void ClearOnStart(HittableSurfaceParticlePoolingItemController poolController)
        {
            particlePoolingController = poolController;
            ReloadParticle();
        }


        void GetParticleSystems()
        {
            particleSystems.Clear();

            ParticleSystem[] subParticles = GetComponentsInChildren<ParticleSystem>();
            foreach (ParticleSystem item in subParticles)
            {
                particleSystems.Add(item);
            }
        }

        public void PlayParticles()
        {
            for (int i = 0; i < particleSystems.Count; i++)
            {
                particleSystems[i].Play();
            }
            StartCoroutine("ReloadParticleTimer");
        }

        void ReloadParticle()
        {
            for (int i = 0; i < particleSystems.Count; i++)
            {
                particleSystems[i].Stop();
                particleSystems[i].Clear();
            }

            particlePoolingController.ReIntergratePoolableObject(this);
        }

        IEnumerator ReloadParticleTimer()
        {
            yield return new WaitForSeconds(reloadParticleTime);
            ReloadParticle();
        }

        private void Reset()
        {
            GetParticleSystems();
        }
    }
}