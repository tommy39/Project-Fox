using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.Player
{
    public class RagdollComponentController : MonoBehaviour
    {
        public Rigidbody localRigidBody;
        public Collider localCollider;
        public CharacterJoint localCharacterJoint;

        private void Start()
        {
            RagdollController parentRagdollController = GetComponentInParent<RagdollController>();
            parentRagdollController.AddRagdollComponent(this);
            DisableRagdoll();
        }

        public void EnableRagdoll()
        {
            localCollider.enabled = true;
            localRigidBody.isKinematic = false;
        }

        public void DisableRagdoll()
        {
            localCollider.enabled = false;
            localRigidBody.isKinematic = true;
        }
        void Reset()
        {
            localRigidBody = GetComponent<Rigidbody>();
            localCollider = GetComponent<Collider>();
            localCharacterJoint = GetComponent<CharacterJoint>();
        }

        public void DestroyRagdollComponent()
        {
            if (localCharacterJoint != null)
            {
                Destroy(localCharacterJoint);
            }

            Destroy(localCollider);
            Destroy(localRigidBody);
            Destroy(this);
        }
    }
}