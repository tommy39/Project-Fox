using IND.UI;
using Photon.Pun;
using UnityEngine;
using Cinemachine;

namespace IND.PlayerSys
{
    public class PlayerAimController : MonoBehaviourPun
    {

        [SerializeField] private PlayerAimData aimData;
        public bool isAiming = false;
        public LayerMask aimLayerMasks;
        public LayerMask hittableSurfaces;
        [SerializeField] private LayerMask aimWallCollisionLayerMasks;
        [SerializeField] private GameObject aimTargetPrefab;
        [SerializeField] private GameObject aimTargetLinePrefab;
        [SerializeField] private GameObject blockedAimTargetLinePrefab;
        [SerializeField] private GameObject aimRadiusPrefab;
        [HideInInspector] public Transform aimTarget;
        private LineRenderer aimTargetLineRenderer;
        private LineRenderer blockedAimTargetLineRenderer;
        private MeshRenderer aimTargetMeshRenderer;
        public AimRadiusController aimRadiusController;
        private PlayerCameraAimController aimCameraController;
        private CinemachineVirtualCamera virtualCam;
        private CinemachineFramingTransposer framingTransposer;

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
        public bool isPlayerTooCloseToWall = false;
        private void Awake()
        {
            inventoryController = GetComponent<PlayerInventoryController>();
            cam = FindObjectOfType<CamController>().GetComponent<Camera>();
            animController = GetComponent<PlayerAnimController>();
            movementController = GetComponent<PlayerMovementController>();
            aimCursorUI = FindObjectOfType<AimCursorUIManager>();
            regularAimCursorController = FindObjectOfType<RegularAimCursorController>();
            aimCameraController = GetComponent<PlayerCameraAimController>();
        }

        private void Start()
        {
            if (aimCursorUI == null)
            {
                aimCursorUI = AimCursorUIManager.singleton;
            }

            if (!photonView.IsMine)
                return;

            virtualCam = FindObjectOfType<CinemachineVirtualCamera>();
            framingTransposer = virtualCam.GetCinemachineComponent<CinemachineFramingTransposer>();
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

            if(aimRadiusController == null)
            {
                GameObject geo = Instantiate(aimRadiusPrefab);
                aimRadiusController = geo.GetComponent<AimRadiusController>();
                aimRadiusController.AssignPlayer(this);
            }

            aimTargetMeshRenderer = aimTarget.gameObject.GetComponentInChildren<MeshRenderer>();
            ToggleAimState(false);
        }

        private void Update()
        {
            if (!photonView.IsMine)
                return;

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

            if(isAiming == false)
            {
                blockedAimTargetLineRenderer.gameObject.SetActive(false);
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
                //  UpdateBlockedAimLineRender();
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

            if (isPlayerTooCloseToWall == true)
            {
                blockedAimTargetLineRenderer.gameObject.SetActive(true);
                blockedAimTargetLineRenderer.SetPosition(0, inventoryController.weaponController.shootpoint.position);
                blockedAimTargetLineRenderer.SetPosition(1, aimTarget.position);


            }
            else if (isAimHittingCollision == false && isPlayerTooCloseToWall == false)
            {
                blockedAimTargetLineRenderer.gameObject.SetActive(false);
            }
            else
            {
                UpdateBlockedAimLineRender();
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
            aimTargetMeshRenderer.gameObject.SetActive(false); //Currently Removed So Setting To False
            aimTargetLineRenderer.gameObject.SetActive(val);

            aimRadiusController.ToggleRenderer(val);

            if (val == false)
            {
                //  regularAimCursorController.gameObject.SetActive(true);
                aimCameraController.SetCameraRegular(virtualCam, framingTransposer);
            }
            else
            {
                //  regularAimCursorController.gameObject.SetActive(false);
                aimCameraController.SetCameraAiming(virtualCam, framingTransposer);
            }
        }
               
        public bool IsAimHittingGround()
        {
            Vector3 rayDir = aimTarget.position - inventoryController.weaponController.shootpoint.position;
            RaycastHit hit;
            if (Physics.Raycast(inventoryController.weaponController.shootpoint.position, rayDir, out hit, 100f, aimLayerMasks))
            {
                if(hit.transform.gameObject.layer == 11) // hit ground
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public void OnDeath()
        {
            if (photonView.IsMine == true)
            {
                Destroy(aimTargetLineRenderer.gameObject);
                Destroy(blockedAimTargetLineRenderer.gameObject);
                Destroy(aimTargetMeshRenderer.gameObject);
                Destroy(aimTarget.gameObject);
            }

            Destroy(this);
        }
    }
}