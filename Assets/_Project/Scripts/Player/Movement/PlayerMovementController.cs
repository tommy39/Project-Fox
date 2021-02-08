using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    public MovementData data;
    public PostureState postureState;
    private Rigidbody rigidBody;
    private Camera cam;

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


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        cam = FindObjectOfType<Camera>();
    }

    private void FixedUpdate()
    {
        HandleMovementInputs();
        CheckMovementInputs();

        camForward = Vector3.Scale(cam.transform.up, new Vector3(1, 0, 1)).normalized;
        move = verticalInput * camForward + horizontalInput * cam.transform.right;


        if (move.magnitude > 1)
        {
            move.Normalize();
        }

        MovePlayer(move);


        moveInput = new Vector3(horizontalInput, 0f, verticalInput);
        moveVelocity = moveInput * data.standingJogSpeed;
        rigidBody.velocity = moveVelocity;
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
}
