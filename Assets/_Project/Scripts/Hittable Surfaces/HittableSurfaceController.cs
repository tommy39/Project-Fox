using Photon.Pun;
using System.Collections;
using UnityEngine;

namespace IND.HittableSurfaces
{
    [RequireComponent(typeof(PhotonView))]
    public class HittableSurfaceController : MonoBehaviour
    {
        [SerializeField] private HittableSurfaceType surfaceType;

        [HideInInspector] public PhotonView photonView;

        private void Start()
        {
            photonView = GetComponent<PhotonView>();
        }

        public void OnObjectHit(Vector3 hitPosition, Vector3 direction)
        {
            object[] objectHitData = { hitPosition, direction };
            photonView.RPC("ObjectHit", RpcTarget.All, objectHitData);
        }

        [PunRPC]
        private void ObjectHit(Vector3 hitPosition, Vector3 direction)
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