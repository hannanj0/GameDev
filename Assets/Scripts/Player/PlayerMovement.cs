using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private PlayerState playerState;

    public float speed = 5.0f;
    public float runMultiplier = 1.5f;
    public Transform cameraTransform;

    private Vector2 moveInput;
    private Vector3 moveDirection;
    private bool isRunning = false;
    private bool canSprint = true; 
    private bool isJumping = false;
    private const float ColliderExtraMargin = 0.1f; // Example value, adjust as needed
    public float maxSlopeAngle = 45.0f;
    public float slopeCheckDistance = 1.0f;

    private Rigidbody rb;
    private PlayerControls controls;
    private Animator animator;

    public float jumpForce = 7.0f;
    private float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    public float airControlFactor = 0.5f;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        animator = GetComponent<Animator>();
        controls = InputManager.Instance.Controls;

        controls.Gameplay.Move.performed += OnMovePerformed;
        controls.Gameplay.Move.canceled += OnMoveCanceled;
        controls.Gameplay.Sprint.performed += OnSprintPerformed;
        controls.Gameplay.Sprint.canceled += OnSprintCanceled;
        controls.Gameplay.Jump.performed += OnJumpPerformed;

        playerState = GetComponent<PlayerState>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Update()
    {
        UpdateMoveDirection();
        UpdateAnimation();

        if (playerState.currentHunger == 0 && isRunning)
        {
            isRunning = false;
        }
    }

    private void FixedUpdate()
    {
        UpdatePosition();

        if (isJumping)
        {
            if (IsGrounded())
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
                isJumping = false;
            }
        }
        else
        {
            if (IsGrounded() && animator.GetBool("isJumping"))
            {
                animator.SetBool("isJumping", false);
            }
        }
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Jump input received");
        Jump();
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            Debug.Log("Attempting to jump");
            animator.SetBool("isJumping", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            isJumping = true;
        }
        else
        {
            Debug.Log("Player tried to jump but wasn't grounded");
        }
    }

    private bool IsGrounded()
    {
        Vector3 origin = transform.position + (Vector3.up * 0.1f);
        float checkDistance = groundCheckDistance + 0.1f;

        Debug.DrawRay(origin, Vector3.down * checkDistance, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.down, out hit, checkDistance, groundLayer))
        {
            return true;
        }

        return false;
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        moveInput = Vector2.zero;
    }

    private void OnSprintPerformed(InputAction.CallbackContext context)
    {
        isRunning = context.ReadValueAsButton() && canSprint && playerState.currentHunger > 0;
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        isRunning = false;
    }

    private void UpdateMoveDirection()
    {
        Vector3 directionRelativeToCamera = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        moveDirection = cameraTransform.TransformDirection(directionRelativeToCamera);
        moveDirection.y = 0;
    }

    private void UpdateAnimation()
    {
        Vector3 localMoveDirection = transform.InverseTransformDirection(moveDirection);
        animator.SetFloat("MoveX", localMoveDirection.x);
        animator.SetFloat("MoveZ", localMoveDirection.z);
        animator.SetBool("isRunning", isRunning);

        bool falling = rb.velocity.y < 0 && !IsGrounded();
        animator.SetBool("isFalling", falling);

        if (IsGrounded())
        {
            animator.SetBool("isFalling", false);
        }
    }

    private bool IsCollidingWithBarrier(Vector3 movement)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, movement.normalized, out hit, movement.magnitude + ColliderExtraMargin, groundLayer))
        {
            float slopeAngle = Vector3.Angle(Vector3.up, hit.normal);
            if (slopeAngle <= maxSlopeAngle)
            {
                return false;
            }

            Debug.Log("Collision detected with steep barrier");
            return true;
        }
        return false;
    }

    private void UpdatePosition()
    {
        float currentSpeed = CalculateCurrentSpeed();
        Vector3 movement = moveDirection * currentSpeed * Time.fixedDeltaTime;

        if (IsGrounded())
        {
            if (!IsCollidingWithBarrier(movement))
            {
                rb.MovePosition(rb.position + movement);
            }
        }
        else
        {
            Vector3 airMovement = moveDirection * currentSpeed * airControlFactor * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + airMovement);
        }
    }

    private float CalculateCurrentSpeed()
    {
        if (isRunning && canSprintOnSlope())
        {
            return speed * runMultiplier;
        }
        return speed;
    }

    private bool canSprintOnSlope()
    {
        if (!isRunning) return true;

        if (IsNearSlopeEdge())
        {
            return CheckSlopeConditionsForSprinting();
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, slopeCheckDistance, groundLayer))
        {
            float slopeAngle = Vector3.Angle(Vector3.up, hit.normal);
            return slopeAngle <= maxSlopeAngle;
        }

        return false;
    }

    private bool IsNearSlopeEdge()
    {
        // Implement logic to check if near the edge of a slope
        // ...

        return false; // Placeholder return
    }

    private bool CheckSlopeConditionsForSprinting()
    {
        // Implement logic to check slope conditions
        // ...

        return true; // Placeholder return
    }

    public void UpdateSprinting(bool sprintAllowed)
    {
        canSprint = sprintAllowed;
    }
}
