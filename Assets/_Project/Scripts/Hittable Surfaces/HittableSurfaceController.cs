using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.HittableSurfaces
{
    public class HittableSurfaceController : MonoBehaviour
    {
        [SerializeField] private HittableSurfaceType surfaceType;

        public void ObjectHit(Vector3 hitPosition, Vector3 direction)
        {
            HittableSurfaceParticleController particleController = null;
            switch (surfaceType)
            {
                case HittableSurfaceType.DIRT:
                    particleController = HittableSurfacesParticlePoolingManager.singleton.dirtParticles.GetPoolableObject();
                    break;
                case HittableSurfaceType.WOOD:
                    break;
                case HittableSurfaceType.BLOOD:
                    break;
                default:
                    break;
            }

            particleController.transform.position = hitPosition;
            particleController.transform.rotation = Quaternion.LookRotation(direction);
            StartCoroutine(TimerToReAddToPool(particleController));
        }

        private IEnumerator TimerToReAddToPool(HittableSurfaceParticleController particleController)
        {
            yield return new WaitForSeconds(1.5f);
            switch (surfaceType)
            {
                case HittableSurfaceType.DIRT:
                    HittableSurfacesParticlePoolingManager.singleton.dirtParticles.ReIntergratePoolableObject(particleController);
                    break;
                case HittableSurfaceType.WOOD:
                    break;
                case HittableSurfaceType.BLOOD:
                    break;
                default:
                    break;
            }
        }
    }
}