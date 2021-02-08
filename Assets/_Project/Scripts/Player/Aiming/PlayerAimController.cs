using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.Weapons;

namespace IND.Player
{
    public class PlayerAimController : MonoBehaviour
    {

        public bool isAiming = false;
        public LayerMask aimLayerMasks;
        [SerializeField] private GameObject aimTargetPrefab;
        [HideInInspector] public Transform aimTarget;

        private PlayerInventoryController inventoryController;
        private PlayerAnimController animController;
        private Camera cam;

        private Ray rayCastPoint;
        private RaycastHit rayHitPoint;

        private void Awake()
        {
            inventoryController = GetComponent<PlayerInventoryController>();
            cam = FindObjectOfType<Camera>();
            animController = GetComponent<PlayerAnimController>();
        }

        private void Start()
        {
            if (aimTarget == null)
            {
                GameObject aimTargetGeo = new GameObject();
                aimTargetGeo.name = "Aim Target " + "For " + gameObject.name;
                aimTarget = aimTargetGeo.transform;
            }
        }

        private void Update()
        {
            HandleAimState();
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
            if (Physics.Raycast(rayCastPoint, out rayHitPoint, 100f, aimLayerMasks))
            {
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