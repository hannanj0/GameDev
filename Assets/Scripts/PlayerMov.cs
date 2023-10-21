using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMov : MonoBehaviour
{
    public float speed = 5.0f;
    public Vector2 moveValue;

    private Rigidbody rb;
    public Transform cameraTransform;

    private Animator animator; // Add this

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>(); // Initialize the Animator
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = cameraTransform.forward * vertical + cameraTransform.right * horizontal;
        direction.y = 0;
      
    }
    private void OnMove(InputValue value)
    {
        moveValue = value.Get<Vector2>();
    }

    private void FixedUpdate()
    {
        // Get the camera's forward and right vectors
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Ignore the vertical component of the camera's forward vector
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 movement = (cameraForward * moveValue.y + cameraRight * moveValue.x).normalized;

        // Only apply movement if there's input
        if (movement != Vector3.zero)
        {
            // Adjust the movement vector for speed and time
            movement *= speed * Time.fixedDeltaTime;

            // Move the character
            rb.MovePosition(transform.position + movement);
        }
    }
}
