using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IND.UI;
using IND.Weapons;

namespace IND.Player
{
    public class PlayerAimController : MonoBehaviour
    {

        public bool isAiming = false;
        public LayerMask aimLayerMasks;
        [SerializeField] private GameObject aimTargetPrefab;
        [SerializeField] private GameObject aimTargetLinePrefab;
        [HideInInspector] public Transform aimTarget;
        private LineRenderer aimTargetLineRenderer;
        private MeshRenderer aimTargetMeshRenderer;

        private PlayerInventoryController inventoryController;
        private PlayerMovementController movementController;
        private PlayerAnimController animController;
        private AimCursorUIManager aimCursorUI;
        private RegularAimCursorController regularAimCursorController;
        private Camera cam;

        private Ray rayCastPoint;
        private RaycastHit rayHitPoint;


        private void Awake()
        {
            inventoryController = GetComponent<PlayerInventoryController>();
            cam = FindObjectOfType<Camera>();
            animController = GetComponent<PlayerAnimController>();
            movementController = GetComponent<PlayerMovementController>();
            aimCursorUI = FindObjectOfType<AimCursorUIManager>();
            regularAimCursorController = FindObjectOfType<RegularAimCursorController>();
        }

        private void Start()
        {
            if (aimTarget == null)
            {
                GameObject aimTargetGeo = Instantiate(aimTargetPrefab);
                aimTargetGeo.name = "Aim Target " + "For " + gameObject.name;
                aimTarget = aimTargetGeo.transform;
            }

            if (aimTargetLineRenderer == null)
            {
                GameObject geo = Instantiate(aimTargetLinePrefab);
                aimTargetLineRenderer = geo.GetComponent<LineRenderer>();

            }

            aimTargetMeshRenderer = aimTarget.gameObject.GetComponentInChildren<MeshRenderer>();
            ToggleAimState(false);
        }

        private void Update()
        {
            HandleAimState();
        }

        private void HandleAimState()
        {
            if (isAiming == false && movementController.isSprinting == true)
            {
                return;
            }
            else if (isAiming == false && movementController.IsMovingInProne() == true)
            {
                return;
            }
            else if (isAiming == false && movementController.isSprinting == false && movementController.IsMovingInProne() == false)
            {
                if (Input.GetMouseButton(1))
                {
                    ToggleAimState(true);
                }
            }

            aimCursorUI.UpdatePosition(aimTarget.position);

            if (isAiming == true)
            {
                if (movementController.isSprinting == true || movementController.IsMovingInProne() == true)
                {
                    ToggleAimState(false);
                    return;
                }

                UpdateAimTargetPosition();
                UpdateAimLineRender();

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

        private void UpdateAimLineRender()
        {
            aimTargetLineRenderer.SetPosition(0, inventoryController.weaponController.shootpoint.position);
            aimTargetLineRenderer.SetPosition(1, aimTarget.position);
        }

        private void ToggleAimState(bool val)
        {
            isAiming = val;
            animController.SetAnimBool(PlayerAnimatorStatics.isAimingAnimBool, val);
            aimTargetMeshRenderer.gameObject.SetActive(val);
            aimTargetLineRenderer.gameObject.SetActive(val);
            aimCursorUI.gameObject.SetActive(val);

            if(val == false)
            {
                regularAimCursorController.gameObject.SetActive(true);
            }
            else
            {
                regularAimCursorController.gameObject.SetActive(false);
            }
        }
    }
}