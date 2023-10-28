using UnityEngine;

public class PlayerMovementFinalGame : MonoBehaviour
{
    public float speed = 5.0f;
    public float runMultiplier = 2.0f;

    private Vector3 moveDirection;
    private Rigidbody rb;
    public Transform cameraTransform;
    private Animator animator;
    private bool isRunning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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

        // Convert world moveDirection to local direction relative to the player
        Vector3 localDirection = transform.InverseTransformDirection(moveDirection);
        
        // Set animator MoveX and MoveZ
        animator.SetFloat("MoveX", localDirection.x);
        animator.SetFloat("MoveZ", localDirection.z);

        // Check if the player is running
        if (directionRelativeToCamera.magnitude > 0.1f && Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            animator.SetBool("isRunning", true);
        }
        else
        {
            isRunning = false;
            animator.SetBool("isRunning", false);
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
}
