using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Movment")] private float movementSpeed;
    [SerializeField] private float walkSpeed;
    [SerializeField] private float movementDrag;
    private static float speedMultiplier = 1.5f;
    private Vector3 moveDir;

    [Header("Jump")] [SerializeField] private bool canJump = true;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airSpeed;
    [Header("Grounded")] [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask ground;
    private static float plusGroundConstant = 0.55f;
    [Header("Crouching")] [SerializeField] private float crouchSpeed;
    [SerializeField] private float crouchHeight;
    private float startHeight;
    [SerializeField] private float turnSpeed;
    [SerializeField] private Transform orientation;

    [SerializeField] private Controls playerControls;
    [SerializeField] private Camera playerCamera;

    private float horizontalInput;
    private float verticalInput;

    [SerializeField] private MovementState movementState;
    private Rigidbody rb;
    private Vector2 mousePos;
    [SerializeField] private GameObject playerBody;

    private enum MovementState
    {
        WALKING,
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
        RotatePlayer();
    }

    private void RotatePlayer()
    {
        Ray mouseRay = playerCamera.ScreenPointToRay(Input.mousePosition);
        Plane p = new Plane(Vector3.up, playerBody.transform.position);
        if (p.Raycast(mouseRay, out float hitDist))
        {
            Vector3 hitPoint = mouseRay.GetPoint(hitDist);
            playerBody.transform.LookAt(hitPoint);
        }
    }

    #region Getters

    public float GetMovementSpeed()
    {
        return rb.velocity.magnitude;
    }

    public Vector3 GetMoveDir()
    {
        return moveDir;
    }

    #endregion

    private void StateHandler()
    {
        //Cambiar a un Switch
        if (GroundCheck())
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
        Debug.DrawRay(orientation.position, Vector3.down * (playerHeight / 2 + plusGroundConstant), Color.blue);
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
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (Input.GetKeyDown(playerControls.crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchHeight, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
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
        moveDir = (orientation.forward * verticalInput + orientation.right * horizontalInput).normalized;

        if (GroundCheck())
            rb.AddForce(moveDir * (movementSpeed * speedMultiplier), ForceMode.Force);
        else
            rb.AddForce(moveDir * (movementSpeed * speedMultiplier * airSpeed), ForceMode.Force);
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
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        canJump = true;
    }
}