using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.HittableSurfaces;
using IND.Player;
using IND.UI;

namespace IND.Weapons
{
    public class WeaponController : MonoBehaviour
    {
        public WeaponData weaponData;

        [SerializeField] private WeaponMuzzleFlashController muzzleFlashController;
        public Transform shootpoint;

        private PlayerAnimController animController;
        private PlayerAimController aimController;
        private GameplayHUDManager hudManager;

        [HideInInspector] public int currentMagazineAmmoAmount;

        private RaycastHit rayHit;
        private Vector3 rayDirection;

        private void Awake()
        {
            aimController = GetComponentInParent<PlayerAimController>();
            animController = GetComponentInParent<PlayerAnimController>();
            hudManager = FindObjectOfType<GameplayHUDManager>();
        }

        public void Init()
        {
            currentMagazineAmmoAmount = weaponData.maxMagazineAmmo;
        }

        private void Update()
        {
            HandleShootingInput();
            HandleReloadInput();
        }

        private void HandleShootingInput()
        {
            if (aimController.isAiming == false)
                return;

            if(currentMagazineAmmoAmount == 0)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                FireWeapon();
            }
        }

        private void HandleReloadInput()
        {
            if(currentMagazineAmmoAmount != weaponData.maxMagazineAmmo)
            {
                if(Input.GetKeyDown(KeyCode.R))
                {
                    BeginReload();
                }
            }
        }

        public virtual void FireWeapon()
        {
            currentMagazineAmmoAmount--;
            animController.SetAnimBool(PlayerAnimatorStatics.isFiringAnimBool, true);
            muzzleFlashController.PlayFireParticles();
            StartCoroutine(StopWeaponFiringTimer());

            rayDirection = aimController.aimTarget.position - shootpoint.position;

            if (Physics.Raycast(shootpoint.position, rayDirection, out rayHit, 100f, aimController.aimLayerMasks))
            {
                HittableSurfaceController hitSurfaceController = null;
                hitSurfaceController = rayHit.transform.GetComponent<HittableSurfaceController>();
                if(hitSurfaceController != null)
                {
                    hitSurfaceController.ObjectHit(rayHit.point, -rayHit.normal);
                }

                HealthHitboxController hitboxController = null;
                hitboxController = rayHit.transform.GetComponent<HealthHitboxController>();
                if(hitboxController != null)
                {
                    hitboxController.OnHitboxHit(this);
                }
            }

            hudManager.UpdateWeaponAmmoUI();
        }

        private void BeginReload()
        {
            animController.SetAnimBool(PlayerAnimatorStatics.isReloadingAnimBool, true);
            StartCoroutine(ReloadTimer());
        }

        public virtual IEnumerator StopWeaponFiringTimer()
        {
            yield return new WaitForSeconds(weaponData.firingWeaponAnimTimer);
            animController.SetAnimBool(PlayerAnimatorStatics.isFiringAnimBool, false);
        }

        private IEnumerator ReloadTimer()
        {
            yield return new WaitForSeconds(weaponData.reloadDuration);
            currentMagazineAmmoAmount = weaponData.maxMagazineAmmo;
            animController.SetAnimBool(PlayerAnimatorStatics.isReloadingAnimBool, false);
            hudManager.UpdateWeaponAmmoUI();
        }
    }
}