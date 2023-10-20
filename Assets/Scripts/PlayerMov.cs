using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMov : MonoBehaviour
{
    public float speed = 5.0f;
    public float gravity = 9.81f;
    private Vector3 velocity;
    private CharacterController characterController;
    public Transform cameraTransform;
    private Animator animator; // Add this

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>(); // Initialize the Animator
    }

    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direction = cameraTransform.forward * vertical + cameraTransform.right * horizontal;
        direction.y = 0;
        direction = direction.normalized;

        // Animation logic
        bool isMoving = direction.magnitude >= 0.1f;
        animator.SetBool("IsWalking", isMoving); // Set the IsWalking parameter

        if (isMoving)
        {
            characterController.Move(direction * speed * Time.deltaTime);
        }

        if (characterController.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y -= gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
