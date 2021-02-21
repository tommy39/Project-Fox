using IND.HittableSurfaces;
using IND.Network;
using IND.PlayerSys;
using IND.UI;
using Photon.Pun;
using System.Collections;
using UnityEngine;

namespace IND.Weapons
{
    public class WeaponController : MonoBehaviour
    {
        public WeaponData weaponData;

        [SerializeField] private WeaponMuzzleFlashController muzzleFlashController;
        public Transform shootpoint;

        private PlayerAnimController animController;
        public PlayerAimController aimController;
        private GameplayHUDManager hudManager;

        [HideInInspector] public PhotonView photonView;

        [HideInInspector] public int currentMagazineAmmoAmount;

        private bool hasFireRateCooldown = false;

        private RaycastHit rayHit;
        private Vector3 rayDirection;

        private void Awake()
        {
            photonView = GetComponent<PhotonView>();
            hudManager = FindObjectOfType<GameplayHUDManager>();
        }

        private void Start()
        {
            currentMagazineAmmoAmount = weaponData.maxMagazineAmmo;

            ClientController client = ClientManager.singleton.GetClientByID(photonView.ControllerActorNr);
            PlayerInventoryController invController = client.data.characterController.GetComponent<PlayerInventoryController>();
            transform.SetParent(invController.rightHandTransform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            invController.weaponController = this;
            aimController = GetComponentInParent<PlayerAimController>();
            animController = GetComponentInParent<PlayerAnimController>();

            hudManager.AssignPlayer(invController);
        }



        private void Update()
        {
            if (!photonView.IsMine)
                return;

            HandleShootingInput();
            HandleReloadInput();
        }

        private void HandleShootingInput()
        {
            if (aimController.isAiming == false)
                return;

            if (currentMagazineAmmoAmount == 0)
            {
                return;
            }

            if (hasFireRateCooldown == true)
                return;

            if (aimController.isPlayerTooCloseToWall == true)
                return;

            switch (weaponData.fireRateMode)
            {
                case WeaponFireRate.SEMI_AUTO:
                    HandleSemiAutoFireInput();
                    break;
                case WeaponFireRate.AUTO:
                    HandleFullAutoFireInput();
                    break;
            }
        }

        private void HandleSemiAutoFireInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                FireWeapon();
            }
        }

        private void HandleFullAutoFireInput()
        {
            if (Input.GetMouseButton(0))
            {
                FireWeapon();
            }
        }

        private void HandleReloadInput()
        {
            if (currentMagazineAmmoAmount != weaponData.maxMagazineAmmo)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    BeginReload();
                }
            }
        }

        public virtual void FireWeapon()
        {
            currentMagazineAmmoAmount--;
            animController.SetAnimBool(PlayerAnimatorStatics.isFiringAnimBool, true);
            photonView.RPC("SendFireParticles", RpcTarget.All);
            StartCoroutine(StopWeaponFiringTimer());
            StartCoroutine(AttackCooldownTimer());
            hasFireRateCooldown = true;
            rayDirection = aimController.aimTarget.position - shootpoint.position;

            switch (weaponData.bulletType)
            {
                case WeaponBulletType.SINGLE:
                    FireSingleBullet();
                    break;
                case WeaponBulletType.SPREAD:
                    FireSpreadBullet();
                    break;
            }


            hudManager.UpdateWeaponAmmoUI();
        }

        [PunRPC]
        private void SendFireParticles()
        {
            muzzleFlashController.PlayFireParticles();
        }

        private void FireSingleBullet()
        {
            if (Physics.Raycast(shootpoint.position, rayDirection, out rayHit, 100f, aimController.aimLayerMasks))
            {
                OnRayHitObject(rayHit);
            }
        }

        private void FireSpreadBullet()
        {
            if (Physics.Raycast(shootpoint.position, rayDirection, out rayHit, 100f, aimController.aimLayerMasks))
            {
                for (int i = 0; i < weaponData.bulletAmounts; i++)
                {
                    //Get Hit Point
                    Vector3 hitTarget = rayHit.point;

                    //Foreach bullet get random pos for x,y,z within a max distance 
                    float x = Random.Range(hitTarget.x - weaponData.spreadAngle, hitTarget.x + weaponData.spreadAngle);
                    float y = Random.Range(hitTarget.y - weaponData.spreadAngle, hitTarget.y + weaponData.spreadAngle);
                    float z = Random.Range(hitTarget.z - weaponData.spreadAngle, hitTarget.z + weaponData.spreadAngle);

                    //Calculate Direction to new destination from shootpoint 
                    Vector3 newHitTarget = new Vector3(x, y, z);
                    rayDirection = newHitTarget - shootpoint.position;
                    if (Physics.Raycast(shootpoint.position, rayDirection, out rayHit, 100f, aimController.aimLayerMasks))
                    {
                        OnRayHitObject(rayHit);
                    }
                }
            }
        }

        private void OnRayHitObject(RaycastHit hit)
        {
            HittableSurfaceController hitSurfaceController = null;
            hitSurfaceController = hit.transform.GetComponent<HittableSurfaceController>();
            if (hitSurfaceController != null)
            {
                hitSurfaceController.OnObjectHit(rayHit.point, -rayHit.normal);
            }

            HealthHitboxController hitboxController = null;
            hitboxController = hit.transform.GetComponent<HealthHitboxController>();
            if (hitboxController != null)
            {
                hitboxController.OnHitboxHit(this);
            }

            if (hitboxController == null)
            {

            }
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

        private IEnumerator AttackCooldownTimer()
        {
            yield return new WaitForSeconds(weaponData.fireRate);
            hasFireRateCooldown = false;
        }
    }
}