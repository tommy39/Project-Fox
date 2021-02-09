using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.Weapons;

namespace IND.Player
{
    public class HealthHitboxController : MonoBehaviour
    {
        private HealthController parentHealthController;

        private void Start()
        {
            parentHealthController = GetComponentInParent<HealthController>();
            parentHealthController.AddChildHitbox(this);
        }

        public void OnHitboxHit(WeaponController weapon)
        {
            parentHealthController.TakeDamage(weapon);
        }
    }
}