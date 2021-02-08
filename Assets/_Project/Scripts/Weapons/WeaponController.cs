using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.Player;

namespace IND.Weapons
{
    public class WeaponController : MonoBehaviour
    {
        public WeaponData weaponData;

        [SerializeField] private WeaponMuzzleFlashController muzzleFlashController;

        private PlayerAnimController animController;

        private void Awake()
        {
            animController = GetComponentInParent<PlayerAnimController>();
        }

        public virtual void FireWeapon()
        {
            animController.SetAnimBool(PlayerAnimatorStatics.isFiringAnimBool, true);
            muzzleFlashController.PlayFireParticles();
            StartCoroutine(StopWeaponFiringTimer());
        }

        public virtual IEnumerator StopWeaponFiringTimer()
        {
            yield return new WaitForSeconds(weaponData.firingWeaponAnimTimer);
            animController.SetAnimBool(PlayerAnimatorStatics.isFiringAnimBool, false);
        }
    }
}