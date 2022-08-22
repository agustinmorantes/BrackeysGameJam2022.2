using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Serialization;

public class Movement : MonoBehaviour
{
    [Header("Movment")] 
    private float movementSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float movementDrag;
    [SerializeField] private float turnSpeed;
    private static float speedMultiplier = 1.5f;
    private Vector3 moveDir;

    [Header("Jump")] 
    [SerializeField]private bool canJump = true;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airSpeed;
    [Header("Grounded")]
    
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask ground;
    private static float plusGroundConstant = 0.55f;
    [Header("Crouching")]
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchHeight;
    private float startHeight;
    
    [SerializeField] private Transform orientation;

    [SerializeField] private Controls playerControls;
    
    private float horizontalInput;
    private float verticalInput;
    
    [SerializeField] private MovementState movementState;
    private Rigidbody rb;
    private enum MovementState
    {
        WALKING,
        SPRINTING,
        CROUCHING,
        AIR
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerControls = GetComponent<Controls>();
    }

    void Start()
    {
        rb.freezeRotation = true;
        startHeight = transform.localScale.y;
    }

    // Update is called once per frame
    private void Update()
    {
        GetInput();
        SpeedLimiter();
        StateHandler();
    }
    
    private void FixedUpdate()
    {
        MovePlayer();
    }

    #region Getters

    public float GetMovementSpeed()
    {
        return rb.velocity.magnitude;
    }

    #endregion
    private void StateHandler()
    {
        //Cambiar a un Switch
        if (GroundCheck() && Input.GetKey(playerControls.sprintKey))
        {
            movementState = MovementState.SPRINTING;
            movementSpeed = sprintSpeed;
        }
        else if (GroundCheck())
        {
            movementState = MovementState.WALKING;
            movementSpeed = walkSpeed;
        }
        else
        {
            movementState = MovementState.AIR;
        }

        if (Input.GetKeyDown(playerControls.crouchKey))
        {
            movementState = MovementState.CROUCHING;
            movementSpeed = crouchSpeed;
        }
    }
    private bool GroundCheck()
    {
        Debug.DrawRay(orientation.position,Vector3.down * (playerHeight/2 + plusGroundConstant),Color.blue);
        return Physics.Raycast(orientation.position, Vector3.down, playerHeight / 2 + plusGroundConstant, ground);
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        if (Input.GetKey(playerControls.jumpKey) && canJump && GroundCheck())
        {
            canJump = false;
            Jump();
            Invoke(nameof(ResetJump),jumpCooldown);
        }

        if (Input.GetKeyDown(playerControls.crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f,ForceMode.Impulse);
        }
        if (Input.GetKeyUp(playerControls.crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startHeight, transform.localScale.z);
        } 
    }

    private void AddPlayerDrag()
    {
        if (GroundCheck())
        {
            rb.drag = movementDrag;
        }
        else rb.drag = 0;
    }

    private void MovePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (GroundCheck()) 
            rb.AddForce(moveDir.normalized * movementSpeed * speedMultiplier,ForceMode.Force);
        else
            rb.AddForce(moveDir.normalized * movementSpeed * speedMultiplier,ForceMode.Force);
        AddPlayerDrag();
        
    }
    

    private void SpeedLimiter()
    {
        Vector3 flatVector = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVector.magnitude > movementSpeed)
        {
            Vector3 limit = flatVector.normalized * movementSpeed;
            rb.velocity = new Vector3(limit.x, rb.velocity.y, limit.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * jumpForce,ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }
}

