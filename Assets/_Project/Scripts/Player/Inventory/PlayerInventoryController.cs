using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.Weapons;

namespace IND.Player
{
    public class PlayerInventoryController : MonoBehaviour
    {
        public WeaponData weaponData;
        public WeaponController weaponController;

        public Transform rightHandTransform;

        private PlayerAnimController animController;
        private PlayerAimController aimController;

        private void Awake()
        {
            animController = GetComponent<PlayerAnimController>();
        }

        private void Start()
        {
            aimController = GetComponent<PlayerAimController>();

            if (weaponData != null)
            {
                SpawnWeapon();
            }
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
        }

    }
}
