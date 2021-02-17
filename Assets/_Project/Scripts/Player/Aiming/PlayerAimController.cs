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
        [SerializeField] private LayerMask aimWallCollisionLayerMasks;
        [SerializeField] private GameObject aimTargetPrefab;
        [SerializeField] private GameObject aimTargetLinePrefab;
        [SerializeField] private GameObject blockedAimTargetLinePrefab;
        [HideInInspector] public Transform aimTarget;
        private LineRenderer aimTargetLineRenderer;
        private LineRenderer blockedAimTargetLineRenderer;
        private MeshRenderer aimTargetMeshRenderer;

        private PlayerInventoryController inventoryController;
        private PlayerMovementController movementController;
        private PlayerAnimController animController;
        private AimCursorUIManager aimCursorUI;
        private RegularAimCursorController regularAimCursorController;
        private Camera cam;

        private Ray rayMouseAimCastPoint;

        private RaycastHit rayMouseAimHitPoint;
        private RaycastHit rayAimHitPoint;
        private RaycastHit wallCheckRay;
        private bool isAimHittingCollision = false;
        [HideInInspector] public bool isPlayerTooCloseToWall = false;

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
            if(aimCursorUI == null)
            {
                aimCursorUI = AimCursorUIManager.singleton;
            }

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

            if (blockedAimTargetLineRenderer == null)
            {
                GameObject geo = Instantiate(blockedAimTargetLinePrefab);
                blockedAimTargetLineRenderer = geo.GetComponent<LineRenderer>();
                blockedAimTargetLineRenderer.gameObject.SetActive(false);
            }

            aimTargetMeshRenderer = aimTarget.gameObject.GetComponentInChildren<MeshRenderer>();
            ToggleAimState(false);
            Debug.Log(gameObject);
        }

        private void Update()
        {
            HandleAimState();
        }

        private void HandleAimState()
        {
            IsPlayerTooCloseToWallToAim();

            if (isPlayerTooCloseToWall == true)
            {
                if (isAiming == true)
                {
                    animController.SetAnimBool(PlayerAnimatorStatics.isAimingAnimBool, false);
                }
            }
            else
            {
                if (isAiming == true)
                {
                    animController.SetAnimBool(PlayerAnimatorStatics.isAimingAnimBool, true);
                }
            }

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

        private void IsPlayerTooCloseToWallToAim()
        {
            Vector3 rayDir = transform.TransformDirection(Vector3.forward);
            Vector3 castPos = GetWallCheckCastPos();
            if (Physics.Raycast(castPos, rayDir, out wallCheckRay, inventoryController.weaponData.minDistanceFromWallToShoot, aimWallCollisionLayerMasks))
            {
                isPlayerTooCloseToWall = true;
            }
            else
            {
                isPlayerTooCloseToWall = false;
            }
        }

        private Vector3 GetWallCheckCastPos()
        {
            switch (movementController.postureState)
            {
                case PostureState.STANDING:
                    return new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
                case PostureState.CROUCHED:
                    return new Vector3(transform.position.x, transform.position.y + 0.8f, transform.position.z);
                case PostureState.PRONE:
                    return new Vector3(transform.position.x, transform.position.y + 0.2f, transform.position.z);
            }
            return Vector3.zero;
        }

        private void UpdateAimTargetPosition()
        {
            rayMouseAimCastPoint = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(rayMouseAimCastPoint, out rayMouseAimHitPoint, 100f, aimLayerMasks))
            {
                Vector3 targetDestination = rayMouseAimHitPoint.point;
                CalculateRayObjectCollision(targetDestination);
                aimTarget.transform.position = targetDestination;
            }
        }

        private void CalculateRayObjectCollision(Vector3 targetPos)
        {
            Vector3 rayDir = targetPos - inventoryController.weaponController.shootpoint.position;

            //Cast a ray from the Weapon Cast Point to the mouse position 
            if (Physics.Raycast(inventoryController.weaponController.shootpoint.position, rayDir, out rayAimHitPoint, 100f, aimWallCollisionLayerMasks))
            {
                isAimHittingCollision = true;
                blockedAimTargetLineRenderer.gameObject.SetActive(true);
                UpdateBlockedAimLineRender();
            }
            else
            {
                isAimHittingCollision = false;
                blockedAimTargetLineRenderer.gameObject.SetActive(false);
            }
        }

        private void UpdateAimLineRender()
        {
            aimTargetLineRenderer.SetPosition(0, inventoryController.weaponController.shootpoint.position);
            if (isAimHittingCollision == false)
            {
                aimTargetLineRenderer.SetPosition(1, aimTarget.position);
            }
            else
            {
                aimTargetLineRenderer.SetPosition(1, rayAimHitPoint.point);
            }
        }

        private void UpdateBlockedAimLineRender()
        {
            blockedAimTargetLineRenderer.SetPosition(0, rayAimHitPoint.point);
            blockedAimTargetLineRenderer.SetPosition(1, aimTarget.position);
        }

        private void ToggleAimState(bool val)
        {
            isAiming = val;
            animController.SetAnimBool(PlayerAnimatorStatics.isAimingAnimBool, val);
            aimTargetMeshRenderer.gameObject.SetActive(val);
            aimTargetLineRenderer.gameObject.SetActive(val);
            aimCursorUI.gameObject.SetActive(val);

            if (val == false)
            {
                regularAimCursorController.gameObject.SetActive(true);
            }
            else
            {
                regularAimCursorController.gameObject.SetActive(false);
            }
        }

        public void OnDeath()
        {
            Destroy(aimTargetLineRenderer.gameObject);
            Destroy(blockedAimTargetLineRenderer.gameObject);
            Destroy(aimTargetMeshRenderer.gameObject);

            Destroy(this);
        }
    }
}