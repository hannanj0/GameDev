using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5.0f;
    public float runMultiplier = 2.0f;

    private Vector3 moveDirection;
    private Rigidbody rb;
    public Transform cameraTransform;
    private bool isRunning = false;
    private bool isSprinting = true; // Added variable to track sprinting

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputZ = Input.GetAxis("Vertical");

        // Determine movement direction relative to the camera's orientation
        Vector3 directionRelativeToCamera = new Vector3(inputX, 0, inputZ).normalized;
        moveDirection = cameraTransform.TransformDirection(directionRelativeToCamera);
        moveDirection.y = 0;
        moveDirection.Normalize();

        // Check if the player is allowed to sprint
        if (isSprinting && directionRelativeToCamera.magnitude > 0.1f && Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private void FixedUpdate()
    {
        // Adjust speed if running
        float currentSpeed = isRunning ? speed * runMultiplier : speed;

        Vector3 movement = moveDirection * currentSpeed * Time.fixedDeltaTime;

        // Move the character
        rb.MovePosition(transform.position + movement);
    }

    // Function to update sprinting based on hunger
    public void UpdateSprinting(bool canSprint)
    {
        isSprinting = canSprint;
    }
}
