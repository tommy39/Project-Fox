using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.Weapons;
using UnityEngine.UI;
using IND.PlayerSys;

namespace IND.UI {
    public class ChooseableWeaponSlotUIController : MonoBehaviour
    {
        [SerializeField] private Image weaponIconSlot;
        private Outline selectedHighlight;

        public WeaponData assignedWeapon;
        public bool isSelected = false;

        private WeaponLoadoutUIManager weaponLoadoutManagerUI;

        public void Setup(WeaponData targetWeapon)
        {
            assignedWeapon = targetWeapon;
            weaponIconSlot.sprite = targetWeapon.weaponHorizontalIcon;
            isSelected = false;
            GetComponent<Button>().onClick.AddListener(() => { OnPress(); });
            weaponLoadoutManagerUI = WeaponLoadoutUIManager.singleton;
            selectedHighlight = GetComponent<Outline>();
            selectedHighlight.enabled =false;
        }

        public void OnPress()
        {
            if (isSelected == true)
                return;

            weaponLoadoutManagerUI.OnSlotSelected(this);
            selectedHighlight.enabled = true;
            isSelected = true;
        }

        public void DeSelectSlot()
        {
            isSelected = false;
            selectedHighlight.enabled = false;
        }
    }
}