using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IND.Player
{
    public class PlayerMovementController : MonoBehaviour
    {
        public MovementData data;
        public PostureState postureState;
        [HideInInspector] public bool isSprinting = false;

        public CapsuleCollider standingCollider;
        public CapsuleCollider crouchedCollider;
        public CapsuleCollider proneCollider;

        private Rigidbody rigidBody;
        private Camera cam;
        private PlayerAnimController animController;
        private PlayerInventoryController inventoryController;
        private PlayerAimController aimController;


        private Vector3 moveInput;
        private Vector3 camForward;
        private Vector3 move;
        private Vector3 moveVelocity;

        private float verticalNormalizedMoveAmount;
        private float horizontallNormalizedMoveAmount;

        #region Movement Inputs
        private bool isPressingMovementKeys;
        private bool rightMovementInput;
        private bool leftMovementInput;
        private bool forwardMovementInput;
        private bool backwardsMovementInput;
        private float horizontalInput;
        private float verticalInput;
        #endregion

        #region AIM Rotation
        private Vector3 adjustedAimPos;
        private float aimPosAdjustmentAmount = 1f;
        private Vector3 targetRotPoint;
        private Quaternion targetRotation;
        #endregion


        private void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            cam = FindObjectOfType<Camera>();
            animController = GetComponent<PlayerAnimController>();
            inventoryController = GetComponent<PlayerInventoryController>();
            aimController = GetComponent<PlayerAimController>();
        }

        private void Start()
        {
            ChangePostureToStanding();
        }

        private void Update()
        {
            HandleSprintInput();
            HandlePostureInput();
        }

        private void FixedUpdate()
        {
            HandleMovementInputs();
            CheckMovementInputs();
            HandleRotation();

            camForward = Vector3.Scale(cam.transform.up, new Vector3(1, 0, 1)).normalized;
            move = verticalInput * camForward + horizontalInput * cam.transform.right;


            if (move.magnitude > 1)
            {
                move.Normalize();
            }

            MovePlayer(move);


            moveInput = new Vector3(horizontalInput, 0f, verticalInput);
            moveVelocity = moveInput * GetMovementSpeed();
            rigidBody.velocity = moveVelocity;

            UpdateMovementAnims();
        }

        private void HandleSprintInput()
        {

            if (isSprinting == false)
            {

                if (postureState != PostureState.STANDING)
                {
                    return;
                }

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    ToggleSprint(true);
                }
            }
            else
            {
                if (postureState != PostureState.STANDING)
                {
                    ToggleSprint(false);
                    return;
                }

                if (Input.GetKeyUp(KeyCode.LeftShift))
                {
                    ToggleSprint(false);
                }
            }
        }

        private void ToggleSprint(bool val)
        {
            isSprinting = val;
            animController.SetAnimBool(PlayerAnimatorStatics.isSprintingAnimBool, val);
        }

        private float GetMovementSpeed()
        {
            if (isSprinting == true)
            {
                return data.sprintSpeed;
            }

            switch (postureState)
            {
                case PostureState.STANDING:
                    return data.standingJogSpeed;
                case PostureState.CROUCHED:
                    return data.crouchedMovementSpeed;
                case PostureState.PRONE:
                    return data.proneMovementSpeed;
            }
            return 0;
        }

        private void HandleRotation()
        {
            if (isPressingMovementKeys == false && aimController.isAiming == false)
                return;

            if (aimController.isAiming == false)
            {
                aimController.aimTarget.position = GetMovementAimPosition();
            }



            targetRotPoint = new Vector3(aimController.aimTarget.transform.position.x, transform.position.y, aimController.aimTarget.transform.position.z) - transform.position;
            targetRotation = Quaternion.LookRotation(targetRotPoint, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * data.aimRotationSpeed);
        }

        private Vector3 GetMovementAimPosition()
        {
            if (leftMovementInput == true && forwardMovementInput == true) //Forward Left
            {
                adjustedAimPos = new Vector3(transform.position.x - aimPosAdjustmentAmount, 0f, transform.position.z + aimPosAdjustmentAmount);
            }
            else if (leftMovementInput == true && backwardsMovementInput == true) //Backward Left
            {
                adjustedAimPos = new Vector3(transform.position.x - aimPosAdjustmentAmount, 0f, transform.position.z - aimPosAdjustmentAmount);
            }
            else if (rightMovementInput == true && forwardMovementInput == true) //Forward Right
            {
                adjustedAimPos = new Vector3(transform.position.x + aimPosAdjustmentAmount, 0f, transform.position.z + aimPosAdjustmentAmount);
            }
            else if (leftMovementInput == true && backwardsMovementInput == true) //Backward Right
            {
                adjustedAimPos = new Vector3(transform.position.x + aimPosAdjustmentAmount, 0f, transform.position.z - aimPosAdjustmentAmount);
            }
            else if (leftMovementInput == true) //Left
            {
                adjustedAimPos = new Vector3(transform.position.x - aimPosAdjustmentAmount, 0f, transform.position.z);
            }
            else if (rightMovementInput == true) //Right
            {
                adjustedAimPos = new Vector3(transform.position.x + aimPosAdjustmentAmount, 0f, transform.position.z);
            }
            else if (forwardMovementInput == true) //Forward
            {
                adjustedAimPos = new Vector3(transform.position.x, 0f, transform.position.z + aimPosAdjustmentAmount);
            }
            else if (backwardsMovementInput == true) //Back
            {
                adjustedAimPos = new Vector3(transform.position.x, 0f, transform.position.z - aimPosAdjustmentAmount);
            }
            else
            {
                adjustedAimPos = inventoryController.transform.position; //Return In Front Of the Idle Position 
            }

            return adjustedAimPos;
        }

        private void HandleMovementInputs()
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");
        }

        private void CheckMovementInputs()
        {
            if (horizontalInput > 0.2)
            {
                rightMovementInput = true;
            }
            else
            {
                rightMovementInput = false;
            }

            if (horizontalInput < -0.2)
            {
                leftMovementInput = true;
            }
            else
            {
                leftMovementInput = false;
            }

            if (verticalInput > 0.2)
            {
                forwardMovementInput = true;
            }
            else
            {
                forwardMovementInput = false;
            }

            if (verticalInput < -0.2)
            {
                backwardsMovementInput = true;
            }
            else
            {
                backwardsMovementInput = false;
            }

            if (rightMovementInput || leftMovementInput || forwardMovementInput || backwardsMovementInput)
            {
                if (isPressingMovementKeys == false)
                {
                    isPressingMovementKeys = true;
                }
            }
            else
            {
                if (isPressingMovementKeys == true)
                {
                    isPressingMovementKeys = false;
                }
            }
        }

        private void UpdateMovementAnims()
        {
            animController.SetAnimFloat(PlayerAnimatorStatics.verticalMovementInt, verticalNormalizedMoveAmount);
            animController.SetAnimFloat(PlayerAnimatorStatics.horizontalMovementInt, horizontallNormalizedMoveAmount);
        }

        private void MovePlayer(Vector3 move)
        {
            if (move.magnitude > 1)
            {
                move.Normalize();
            }

            moveInput = move;

            ConvertMoveInput();
        }

        void ConvertMoveInput()
        {
            Vector3 localMove = transform.InverseTransformDirection(moveInput);

            horizontallNormalizedMoveAmount = localMove.x;
            verticalNormalizedMoveAmount = localMove.z;
        }

        private void HandlePostureInput()
        {
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                ToggleCrouchPosture();
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                TogglePronePosture();
            }
        }

        private void ToggleCrouchPosture()
        {
            if (postureState == PostureState.CROUCHED)
            {
                ChangePostureToStanding();
                return;
            }

            crouchedCollider.enabled = true;
            proneCollider.enabled = false;
            standingCollider.enabled = false;

            postureState = PostureState.CROUCHED;
            animController.SetAnimInt(PlayerAnimatorStatics.postureStateInt, 1);
        }

        private void TogglePronePosture()
        {
            if (postureState == PostureState.PRONE)
            {
                ChangePostureToStanding();
                return;
            }


            crouchedCollider.enabled = false;
            proneCollider.enabled = true;
            standingCollider.enabled = false;

            postureState = PostureState.PRONE;
            animController.SetAnimInt(PlayerAnimatorStatics.postureStateInt, 2);
        }

        private void ChangePostureToStanding()
        {
            postureState = PostureState.STANDING;
            animController.SetAnimInt(PlayerAnimatorStatics.postureStateInt, 0);

            crouchedCollider.enabled = false;
            proneCollider.enabled = false;
            standingCollider.enabled = true;
        }
    }
}