using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.Ragdolls
{
    public class RagdollManager : MonoBehaviour
    {
        [SerializeField] private int maxActiveRagdolls = 10;
        private int currentActiveRagdolls;

        private List<GameObject> activeRagdolls = new List<GameObject>();

        public static RagdollManager singleton;
        private void Awake()
        {
            singleton = this;
        }

        public void AddRagdoll(GameObject geo)
        {

            activeRagdolls.Add(geo);
            currentActiveRagdolls++;

            if(currentActiveRagdolls > maxActiveRagdolls)
            {
                RemoveRagdoll(activeRagdolls[0]);
            }
        }

        private void RemoveRagdoll(GameObject geo)
        {
            currentActiveRagdolls--;
            activeRagdolls.Remove(geo);
            Destroy(geo);
        }
    }
}