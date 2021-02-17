using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.Weapons;
using IND.UI;

namespace IND.Player
{
    public class PlayerInventoryController : MonoBehaviour
    {
        public WeaponData weaponData;
        public WeaponController weaponController;

        public Transform rightHandTransform;

        private PlayerAnimController animController;
        private PlayerAimController aimController;
        private GameplayHUDManager hudManager;

        private void Awake()
        {
            animController = GetComponent<PlayerAnimController>();
            hudManager = FindObjectOfType<GameplayHUDManager>();
        }

        private void Start()
        {
            aimController = GetComponent<PlayerAimController>();

            if (weaponData != null)
            {
                SpawnWeapon();
            }

            hudManager.AssignPlayer(this);
        }

        private void Update()
        {
        }

        private void SpawnWeapon()
        {
            animController.PlayAnimationHash(PlayerAnimatorStatics.equipWeaponAnimClass);

            GameObject geo = Instantiate(weaponData.modelPrefab, rightHandTransform);
            geo.transform.localPosition = Vector3.zero;
            geo.transform.localRotation = Quaternion.identity;

            weaponController = geo.GetComponent<WeaponController>();
            weaponController.Init();
        }

        public void OnDeath()
        {
            Destroy(weaponController);
            Destroy(this);
        }
    }
}
