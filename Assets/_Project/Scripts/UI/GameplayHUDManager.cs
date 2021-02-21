using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.PlayerSys;
using TMPro;
using UnityEngine.UI;

namespace IND.UI
{
    public class GameplayHUDManager : MonoBehaviour
    {
        [SerializeField] private Image weaponImg;
        [SerializeField] private TextMeshProUGUI weaponNameText;
        [SerializeField] private TextMeshProUGUI weaponAmmoText;

        [SerializeField] private GameObject childObject;

        private PlayerInventoryController playerInventoryController;
        private PlayerMovementController playerMovementController;

        public static GameplayHUDManager singleton;
        private void Awake()
        {
            singleton = this;
            CloseInterface();
        }

        public void AssignPlayer(PlayerInventoryController inventoryController)
        {
            OpenInterface();
            playerInventoryController = inventoryController;
            playerMovementController = inventoryController.GetComponent<PlayerMovementController>();

            weaponImg.sprite = inventoryController.weaponData.weaponDiagonalIcon;
            weaponNameText.text = inventoryController.weaponData.weaponName;
            UpdateWeaponAmmoUI();
        }

        public void UpdateWeaponAmmoUI()
        {
            weaponAmmoText.text = "- " + playerInventoryController.weaponController.currentMagazineAmmoAmount.ToString() + "/" + playerInventoryController.weaponData.maxMagazineAmmo.ToString();
        }

        public void OpenInterface()
        {
            childObject.SetActive(true);
        }

        public void CloseInterface()
        {
            childObject.SetActive(false);
        }
    }
}