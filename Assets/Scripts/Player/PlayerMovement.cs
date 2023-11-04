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

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        // Debug.Log("Sprinting stopped");
    }

    private void UpdateMoveDirection()
    {
        // Convert move input to a world space direction based on the camera's orientation
        Vector3 directionRelativeToCamera = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        moveDirection = cameraTransform.TransformDirection(directionRelativeToCamera);
        moveDirection.y = 0; // Ensure that the player remains grounded
    }

    private void UpdatePosition()
    {
        // Determine if the player can sprint
        isRunning = isRunning && canSprint && moveDirection.magnitude > 0.1f;
        float currentSpeed = isRunning ? speed * runMultiplier : speed;

        // Calculate the movement vector and move the player
        Vector3 movement = moveDirection * currentSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + movement);
    }

    public void UpdateSprinting(bool sprintAllowed)
    {
        canSprint = sprintAllowed;
    }
}
