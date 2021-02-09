using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.Player
{
    public class RagdollController : MonoBehaviour
    {
        private List<RagdollComponentController> childRagdollComponents = new List<RagdollComponentController>();

        public List<Collider> collisionsToDisableWhenEnablingRagdoll = new List<Collider>();

        public void AddRagdollComponent(RagdollComponentController component)
        {
            childRagdollComponents.Add(component);
        }

        public void EnableRagdoll()
        {
            for (int i = 0; i < collisionsToDisableWhenEnablingRagdoll.Count; i++)
            {
                collisionsToDisableWhenEnablingRagdoll[i].enabled = false;
            }

            for (int i = 0; i < childRagdollComponents.Count; i++)
            {
                childRagdollComponents[i].EnableRagdoll();
            }
        }

        public void DisableRagdoll()
        {
            for (int i = 0; i < childRagdollComponents.Count; i++)
            {
                childRagdollComponents[i].DisableRagdoll();
            }
        }
    }
}