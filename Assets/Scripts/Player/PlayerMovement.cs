using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float runMultiplier = 2.0f;
    public Transform cameraTransform;

    private Vector2 moveInput;
    private Vector3 moveDirection;
    private bool isRunning = false;
    private bool canSprint = true;
    private Rigidbody rb;
    private PlayerControls controls;
    private Animator animator; // Added Animator variable

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Initialize the Animator variable
        controls = new PlayerControls();

        // Subscribe to the input system events
        controls.Gameplay.Move.performed += OnMovePerformed;
        controls.Gameplay.Move.canceled += OnMoveCanceled;
        controls.Gameplay.Sprint.performed += OnSprintPerformed;
        controls.Gameplay.Sprint.canceled += OnSprintCanceled;
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
        UpdateAnimation(); // Call UpdateAnimation method
    }

    private void FixedUpdate()
    {
        UpdatePosition();
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
        isRunning = context.ReadValueAsButton();
    }

    private void OnSprintCanceled(InputAction.CallbackContext context)
    {
        isRunning = false;
    }

    private void UpdateMoveDirection()
    {
        Vector3 directionRelativeToCamera = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        moveDirection = cameraTransform.TransformDirection(directionRelativeToCamera);
        moveDirection.y = 0; // Ensure that the player remains grounded
    }

    private void UpdateAnimation()
    {
        // Convert global movement direction to local space
        Vector3 localMoveDirection = transform.InverseTransformDirection(moveDirection);

        // Update the Animator parameters with local direction
        animator.SetFloat("MoveX", localMoveDirection.x);
        animator.SetFloat("MoveZ", localMoveDirection.z);
        animator.SetBool("isRunning", isRunning);
    }


    private void UpdatePosition()
    {
        isRunning = isRunning && canSprint && moveDirection.magnitude > 0.1f;
        float currentSpeed = isRunning ? speed * runMultiplier : speed;
        Vector3 movement = moveDirection * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    public void UpdateSprinting(bool sprintAllowed)
    {
        canSprint = sprintAllowed;
    }
}
