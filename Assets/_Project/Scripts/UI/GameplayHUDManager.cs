using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.Player;
using TMPro;
using UnityEngine.UI;

namespace IND.UI
{
    public class GameplayHUDManager : MonoBehaviour
    {
        [SerializeField] private Image weaponImg;
        [SerializeField] private TextMeshProUGUI weaponNameText;
        [SerializeField] private TextMeshProUGUI weaponAmmoText;

        private PlayerInventoryController playerInventoryController;
        private PlayerMovementController playerMovementController;

        public void AssignPlayer(PlayerInventoryController inventoryController)
        {
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
    }
}