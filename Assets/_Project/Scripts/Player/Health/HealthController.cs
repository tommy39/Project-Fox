using IND.Weapons;
using System.Collections.Generic;
using UnityEngine;

namespace IND.Player
{
    public class HealthController : MonoBehaviour
    {
        public float currentHealth = 100f;
        private List<HealthHitboxController> childHitboxes = new List<HealthHitboxController>();
        private RagdollController ragdollController;

        [SerializeField] private bool isDummy = false;

        private void Awake()
        {
            ragdollController = GetComponent<RagdollController>();
        }

        public void TakeDamage(WeaponController weapon)
        {
            currentHealth -= weapon.weaponData.weaponDamage;
            if (currentHealth <= 0)
            {
                Death(false);
            }

            Debug.Log(weapon.weaponData.weaponDamage + " damage taken");
        }

        public void Death(bool isForced)
        {
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

            if (isForced == false)
            {
                //Open Respawn Interface
            }

            Destroy(this);
        }

        public void AddChildHitbox(HealthHitboxController hitbox)
        {
            childHitboxes.Add(hitbox);
        }
    }
}