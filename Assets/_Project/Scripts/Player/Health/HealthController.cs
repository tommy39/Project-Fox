using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.Weapons;

namespace IND.Player
{
    public class HealthController : MonoBehaviour
    {
        public float currentHealth = 100f;
        private List<HealthHitboxController> childHitboxes = new List<HealthHitboxController>();
        private RagdollController ragdollController;

        private void Awake()
        {
            ragdollController = GetComponent<RagdollController>();
        }

        public void TakeDamage(WeaponController weapon)
        {
            currentHealth -= weapon.weaponData.weaponDamage;
            if (currentHealth <= 0)
            {
                Death();
            }

            Debug.Log(weapon.weaponData.weaponDamage + " damage taken");
        }

        private void Death()
        {
            Debug.Log("Death");
            ragdollController.EnableRagdoll();
        }

        public void AddChildHitbox(HealthHitboxController hitbox)
        {
            childHitboxes.Add(hitbox);
        }
    }
}