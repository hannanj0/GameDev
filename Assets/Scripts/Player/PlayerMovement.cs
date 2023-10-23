using UnityEngine;

public class PlayerMov : MonoBehaviour
{
    public float speed = 5.0f;
    public float runMultiplier = 2.0f; // The speed multiplier when running

    private Vector3 moveValue;
    private Rigidbody rb;
    public Transform cameraTransform;
    private Animator animator;
    private bool isRunning = false; // Add the isRunning boolean

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        moveValue.x = Input.GetAxis("Horizontal");
        moveValue.z = Input.GetAxis("Vertical");

        // Set the isWalking boolean in the animator based on moveValue magnitude
        animator.SetBool("isWalking", moveValue.sqrMagnitude > 0.01f);

        // Check if the player is moving and the shift key is pressed
        if (moveValue.sqrMagnitude > 0.01f && Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            animator.SetBool("isRunning", true); // Assuming you have this parameter in your animator
        }
        else
        {
            isRunning = false;
            animator.SetBool("isRunning", false);
        }
    }

    private void FixedUpdate()
    {
        // Get the camera's forward and right vectors
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Ignore the vertical component of the camera's forward vector
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 movement = (cameraForward * moveValue.z + cameraRight * moveValue.x).normalized;

        // Adjust speed if running
        float currentSpeed = isRunning ? speed * runMultiplier : speed;

        // Only apply movement if there's input
        if (movement.sqrMagnitude > 0.01f)
        {
            // Adjust the movement vector for speed and time
            movement *= currentSpeed * Time.fixedDeltaTime;

            // Move the character
            rb.MovePosition(transform.position + movement);
        }
    }
}
