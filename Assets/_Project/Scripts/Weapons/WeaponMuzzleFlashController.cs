using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.Weapons
{
    public class WeaponMuzzleFlashController : MonoBehaviour
    {
        [SerializeField] protected List<ParticleSystem> particlesToPlay = new List<ParticleSystem>();

        private void Awake()
        {
            for (int i = 0; i < particlesToPlay.Count; i++)
            {
                particlesToPlay[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }

        public void PlayFireParticles()
        {
            for (int i = 0; i < particlesToPlay.Count; i++)
            {
                particlesToPlay[i].Play();
            }
        }

    }
}