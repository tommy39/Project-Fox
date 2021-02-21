using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.PlayerSys;
using IND.Weapons;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

namespace IND.UI
{
    public class WeaponLoadoutUIManager : MonoBehaviourPun
    {
        [SerializeField] private GameObject childObject = default;
        [SerializeField] private GameObject weaponSlotItemPrefab = default;
        [SerializeField] private Transform weaponsListParent = default;
        [SerializeField] private List<WeaponData> availableWeapons = new List<WeaponData>();
        [SerializeField] private Button confirmLoadoutButton = default;

        [SerializeField] private Transform weaponRenderParent = default;
        [SerializeField] private Camera renderCamera = default;
        private GameObject createdWeaponPreview = default;

        [SerializeField] private TextMeshProUGUI selectedWeaponNameText;
        [SerializeField] private TextMeshProUGUI selectedWeaponDescText;

        private List<ChooseableWeaponSlotUIController> createdSlots = new List<ChooseableWeaponSlotUIController>();

        private ChooseableWeaponSlotUIController selectedSlot;

        private PlayerManager playerManager;
        private PlayerLoadoutManager playerLoadoutManager;

        public static WeaponLoadoutUIManager singleton;
        private void Awake()
        {
            singleton = this;
            CreateAllWeapons();
            confirmLoadoutButton.onClick.AddListener(() => { ConfirmLoadout(); });
            CloseInterface();
        }

        private void Start()
        {
            playerLoadoutManager = FindObjectOfType<PlayerLoadoutManager>();
            playerManager = PlayerManager.singleton;
        }

        private void CloseInterface()
        {
            renderCamera.gameObject.SetActive(false);
            childObject.SetActive(false);


            UIManager.singleton.OnUIElementDeActivated();
        }

        public void OpenInterface()
        {
            childObject.SetActive(true);
            renderCamera.gameObject.SetActive(true);

            UIManager.singleton.OnUIElementActivated(childObject);
        }

        private void CreateAllWeapons()
        {
            for (int i = 0; i < availableWeapons.Count; i++)
            {
                CreateWeaponSlotClass(availableWeapons[i]);
            }

            //Select the First Weapon In This List On Start
            createdSlots[0].OnPress();
        }

        private void CreateWeaponSlotClass(WeaponData weapon)
        {
            GameObject geo = Instantiate(weaponSlotItemPrefab, weaponsListParent);
            ChooseableWeaponSlotUIController controller = geo.GetComponent<ChooseableWeaponSlotUIController>();
            createdSlots.Add(controller);
            controller.Setup(weapon);
        }

        public void OnSlotSelected(ChooseableWeaponSlotUIController slot)
        {
            if (selectedSlot == slot)
                return;

            if (selectedSlot != null)
            {
                selectedSlot.DeSelectSlot();
            }

            selectedSlot = slot;
            selectedWeaponNameText.text = slot.assignedWeapon.weaponName;
            selectedWeaponDescText.text = slot.assignedWeapon.weaponDescription;

            //Create 3D Render
            if (createdWeaponPreview != null)
            {
                Destroy(createdWeaponPreview);
            }

            createdWeaponPreview = Instantiate(slot.assignedWeapon.modelPrefab, weaponRenderParent);
            createdWeaponPreview.transform.localPosition = slot.assignedWeapon.renderPos;
            createdWeaponPreview.transform.localRotation = slot.assignedWeapon.renderRot;
            Destroy(createdWeaponPreview.GetComponent<WeaponController>());

        }

        private void ConfirmLoadout()
        {
            photonView.RPC("ChangeWeapon", RpcTarget.All);
            CloseInterface();
            playerManager.SpawnPlayer();
        }

        [PunRPC]
        private void ChangeWeapon()
        {
            playerLoadoutManager.equippedWeapon = selectedSlot.assignedWeapon;
        }
    }
}