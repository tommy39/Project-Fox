using IND.Weapons;
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace IND.PlayerSys
{
    public class HealthController : MonoBehaviourPun
    {
        public float currentHealth = 100f;
        private List<HealthHitboxController> childHitboxes = new List<HealthHitboxController>();
        private RagdollController ragdollController;

        [SerializeField] private bool isDummy = false;

        private void Awake()
        {
            ragdollController = GetComponent<RagdollController>();
        }

        [PunRPC]
        public void TakeDamage(float damage, int clientID)
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Death(false);
            }
        }

        private void Death(bool isForced)
        {
            Destroy(GetComponent<PhotonRigidbodyView>());
            Destroy(GetComponent<Rigidbody>());

            ragdollController.EnableRagdoll();

            if (isDummy == true)
                return;

            //Destroy Components
            GetComponent<PlayerAnimController>().OnDeath();
            GetComponent<PlayerMovementController>().OnDeath();
            GetComponent<PlayerAimController>().OnDeath();
            GetComponent<PlayerInventoryController>().OnDeath();
            Destroy(GetComponent<PlayerController>());
            Destroy(ragdollController);
            Destroy(GetComponent<PhotonTransformView>());

            if (isForced == false)
            {
                //Open Respawn Interface
            }

            Destroy(this);
        }

        public void ExternalDeath()
        {
            Death(true);
        }

        public void AddChildHitbox(HealthHitboxController hitbox)
        {
            childHitboxes.Add(hitbox);
        }
    }
}