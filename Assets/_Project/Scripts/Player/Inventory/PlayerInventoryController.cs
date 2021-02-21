using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.Weapons;
using IND.UI;
using Photon.Pun;

namespace IND.PlayerSys
{
    public class PlayerInventoryController : MonoBehaviourPun
    {
        public WeaponData weaponData;
        public WeaponController weaponController;

        public Transform rightHandTransform;

        private PlayerAnimController animController;
        private PlayerAimController aimController;
        private PlayerLoadoutManager playerLoadoutManager;
        private GameplayHUDManager hudManager;

        private void Awake()
        {
            animController = GetComponent<PlayerAnimController>();
            hudManager = FindObjectOfType<GameplayHUDManager>();
            playerLoadoutManager = FindObjectOfType<PlayerLoadoutManager>();
        }

        private void Start()
        {
            aimController = GetComponent<PlayerAimController>();

            weaponData = playerLoadoutManager.equippedWeapon;


            if (!photonView.IsMine)
                return;

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

            GameObject geo = PhotonNetwork.Instantiate(weaponData.modelPrefab.name, Vector3.zero, Quaternion.identity);
           // Debug.Log(geo.name);
        }

        public void OnDeath()
        {
            Destroy(weaponController);
            Destroy(this);
        }
    }
}
