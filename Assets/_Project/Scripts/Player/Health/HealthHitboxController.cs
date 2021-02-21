using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.Weapons;
using Photon.Pun;

namespace IND.PlayerSys
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
            object[] data = { weapon.weaponData.weaponDamage, weapon.photonView.ControllerActorNr };
            parentHealthController.photonView.RPC("TakeDamage", RpcTarget.All, data);
        }
    }
}