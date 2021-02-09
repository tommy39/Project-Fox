using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.HittableSurfaces
{
    public class HittableSurfacesParticlePoolingManager : MonoBehaviour
    {
        public HittableSurfaceParticlePoolingItemController dirtParticles;

        public static HittableSurfacesParticlePoolingManager singleton;
        private void Awake()
        {
            singleton = this;
        }
    }
}
