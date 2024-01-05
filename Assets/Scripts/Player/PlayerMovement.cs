using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    private PlayerState playerState;

    public float speed = 5.0f;
    public float runMultiplier = 2.0f;
    public Transform cameraTransform;

    private Vector2 moveInput;
    private Vector3 moveDirection;
    private bool isRunning = false;
    private bool canSprint = true;
    private bool isJumping = false;
    private Rigidbody rb;
    private PlayerControls controls;
    private Animator animator;
    public float jumpForce = 7.0f;
    private float groundCheckDistance = 0.2f;
    public LayerMask groundLayer;
    public float maxSlopeAngle = 30.0f;

    public float slopeCheckDistance = 1.0f;

    // Add this variable to control whether the script is active
    private bool isScriptActive = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        controls = InputManager.Instance.Controls;

        controls.Gameplay.Move.performed += OnMovePerformed;
        controls.Gameplay.Move.canceled += OnMoveCanceled;
        controls.Gameplay.Sprint.performed += OnSprintPerformed;
        controls.Gameplay.Sprint.canceled += OnSprintCanceled;
        controls.Gameplay.Jump.performed += OnJumpPerformed;
        playerState = GetComponent<PlayerState>();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void Start()
    {
        StartCoroutine(EnableScriptAfterDelay());
    }

    private IEnumerator EnableScriptAfterDelay()
    {
        yield return new WaitForSeconds(14f); // Change the delay to 14 seconds
        isScriptActive = true; // Enable the entire script functionality
    }

    void Update()
    {
        // Only execute the update logic if the script is active
        if (isScriptActive)
        {
            UpdateMoveDirection();
            UpdateAnimation();

            // Check if hunger is zero and the player is currently sprinting
            if (playerState.currentHunger == 0 && isRunning)
            {
                // Stop sprinting when hunger is zero
                isRunning = false;
            }
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

    private void UpdatePosition()
    {
        float currentSpeed = isRunning ? speed * runMultiplier : speed;
        Vector3 movement = moveDirection * currentSpeed * Time.fixedDeltaTime;

        // Check for barriers or steep slopes before moving
        if (!IsCollidingWithBarrier(movement) && !IsOnSteepSlope())
        {
            rb.MovePosition(rb.position + movement);
        }
    }

    private bool IsCollidingWithBarrier(Vector3 movement)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * groundCheckDistance, movement.normalized, out hit, movement.magnitude + 0.1f, groundLayer))
        {
            // Check if the hit angle is too steep
            if (Vector3.Angle(hit.normal, Vector3.up) > maxSlopeAngle)
            {
                return true; // Too steep, this is a barrier
            }
        }
        return false; // No barrier
    }

    private bool IsOnSteepSlope()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up * groundCheckDistance, Vector3.down, out hit, slopeCheckDistance, groundLayer))
        {
            float slopeAngle = Vector3.Angle(hit.normal, Vector3.up);
            if (slopeAngle > maxSlopeAngle)
            {
                return true; // Too steep, we are on a slope
            }
        }
        return false; // Not on a steep slope
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Jump input received"); // Check if the jump input is detected
        Jump();
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            Debug.Log("Attempting to jump"); // Confirm the jump is attempted
            animator.SetBool("isJumping", true);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange); // Apply the jump force
            isJumping = true;
        }
        else
        {
            Debug.Log("Player tried to jump but wasn't grounded"); // Check if the player is grounded
        }
    }

    private bool IsGrounded()
    {
        // The origin is now at the bottom of the collider, not the center of the GameObject.
        Vector3 origin = transform.position + (Vector3.up * 0.1f);
        float checkDistance = groundCheckDistance + 0.1f; // You might need to adjust this value.
        // Visualize the raycast in the scene view.
        Debug.DrawRay(origin, Vector3.down * checkDistance, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(origin, Vector3.down, out hit, checkDistance, groundLayer))
        {
            //Debug.Log($"Hit: {hit.collider.name}"); // This will tell you what the raycast hit.
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
        if (playerState.currentHunger == 0)
        {
            isRunning = false;
        }
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

        // Check if we are falling (y velocity is negative and we are not on the ground)
        bool falling = rb.velocity.y < 0 && !IsGrounded();
        animator.SetBool("isFalling", falling);

        // Ensure we are not interrupting the jump up animation
        if (IsGrounded())
        {
            animator.SetBool("isFalling", false); // Reset the isFalling when grounded
        }
    }

    // This method allows external scripts to enable or disable sprinting.
    public void UpdateSprinting(bool sprintAllowed)
    {
        canSprint = sprintAllowed;
    }
}
