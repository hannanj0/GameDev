using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float runMultiplier = 2.0f;

    private Vector3 moveDirection;
    private Rigidbody rb;
    public Transform cameraTransform;
    private bool isRunning = false;
    private bool isSprinting = true;

    private PlayerControls controls;
    private Vector2 moveInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        controls = new PlayerControls();

        controls.Gameplay.Move.performed += context => {
            moveInput = context.ReadValue<Vector2>();
            // Debug.Log($"Move input (Vector2): {moveInput}");
        };
        controls.Gameplay.Move.canceled += context => {
            moveInput = Vector2.zero;
            // Debug.Log("Move input reset to zero");
        };

        controls.Gameplay.Sprint.performed += context => {
            isRunning = context.ReadValueAsButton();
            // Debug.Log($"Sprinting: {isRunning}");
        };
        controls.Gameplay.Sprint.canceled += context => {
            isRunning = false;
            Debug.Log("Sprinting stopped");
        };
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
        Vector3 directionRelativeToCamera = new Vector3(moveInput.x, 0, moveInput.y).normalized;
        moveDirection = cameraTransform.TransformDirection(directionRelativeToCamera);
        moveDirection.y = 0;
        moveDirection.Normalize();
    }

    private void FixedUpdate()
    {
        isRunning = isRunning && isSprinting && moveDirection.magnitude > 0.1f;
        float currentSpeed = isRunning ? speed * runMultiplier : speed;
        Vector3 movement = moveDirection * currentSpeed * Time.fixedDeltaTime;
        // Debug.Log($"Movement applied (Vector3): {movement}");

        rb.MovePosition(transform.position + movement);
    }

    public void UpdateSprinting(bool canSprint)
    {
        isSprinting = canSprint;
    }
}