using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IND.HittableSurfaces
{
    public class HittableSurfaceParticlePoolingItemController : MonoBehaviour
    {
        public GameObject objectToPoolPrefab;
        public int amountToSpawn;
        
        public List<PoolableParticleItem> createdPoolableItems = new List<PoolableParticleItem>();

        public void Start()
        {
            createdPoolableItems.Clear();

            for (int i = 0; i < amountToSpawn; i++)
            {
                PoolableParticleItem newItem = new PoolableParticleItem();
                newItem.geo = Instantiate(objectToPoolPrefab);
                newItem.geo.transform.SetParent(transform);
                newItem.geo.transform.localPosition = Vector3.zero;
                newItem.particleController = newItem.geo.GetComponent<HittableSurfaceParticleController>();
                createdPoolableItems.Add(newItem);
                newItem.particleController.ClearOnStart(this);
            }
        }

        public HittableSurfaceParticleController GetPoolableObject()
        {
            if (createdPoolableItems.Count == 0)
            {
                Debug.LogError(gameObject.name + " Has no available objects in the pool currently, increase the amount to spawn");
                return null;
            }

            HittableSurfaceParticleController particleController = createdPoolableItems[0].particleController;
            particleController.transform.SetParent(null);
            particleController.PlayParticles();
            createdPoolableItems.RemoveAt(0);
            return particleController;
        }



        public void ReIntergratePoolableObject(HittableSurfaceParticleController particleController)
        {
            PoolableParticleItem newItem = new PoolableParticleItem();
            newItem.geo = particleController.gameObject;
            newItem.particleController = particleController;

            createdPoolableItems.Add(newItem);
            particleController.transform.SetParent(transform);
            particleController.transform.localPosition = Vector3.zero;
            particleController.transform.localRotation = Quaternion.identity;
        }
    }

    [System.Serializable]
    public class PoolableParticleItem
    {
        public GameObject geo;
        public HittableSurfaceParticleController particleController;
    }
}
