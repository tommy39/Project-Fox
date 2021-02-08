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

        public bool isAiming = false;
        [HideInInspector] public Transform aimTarget;

        public Transform rightHandTransform;
        public LayerMask aimLayerMasks;

        private PlayerAnimController animController;
        private Camera cam;

       private Ray rayCastPoint;
       private RaycastHit rayHitPoint;

        private void Awake()
        {
            animController = GetComponent<PlayerAnimController>();
            cam = FindObjectOfType<Camera>();
        }

        private void Start()
        {
            if(aimTarget == null)
            {
                GameObject aimTargetGeo = new GameObject();
                aimTargetGeo.name = "Aim Target " + "For " + gameObject.name;
                aimTarget = aimTargetGeo.transform;
            }

            if (weaponData != null)
            {
                SpawnWeapon();
            }
        }

        private void Update()
        {
            HandleAimState();
        }

        private void SpawnWeapon()
        {
            animController.PlayAnimationHash(PlayerAnimatorStatics.equipWeaponAnimClass);

            GameObject geo = Instantiate(weaponData.modelPrefab, rightHandTransform);
            geo.transform.localPosition = Vector3.zero;
            geo.transform.localRotation = Quaternion.identity;

            weaponController = geo.GetComponent<WeaponController>();
        }

        private void HandleAimState()
        {
            if (isAiming == false)
            {
                if (Input.GetMouseButtonDown(1))
                {
                    ToggleAimState(true);
                }
            }
            else
            {
                UpdateAimTargetPosition();

                if (Input.GetMouseButtonUp(1))
                {
                    ToggleAimState(false);
                }
            }
        }

        private void UpdateAimTargetPosition()
        {
            rayCastPoint = cam.ScreenPointToRay(Input.mousePosition);
            Debug.Log(1);
            if (Physics.Raycast(rayCastPoint, out rayHitPoint, 100f, aimLayerMasks))
            {
                Debug.Log(2);
                aimTarget.transform.position = rayHitPoint.point;
            }
        }

        private void ToggleAimState(bool val)
        {
            isAiming = val;
            animController.SetAnimBool(PlayerAnimatorStatics.isAimingAnimBool, val);
        }
    }
}
