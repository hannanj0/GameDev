using UnityEngine;
using UnityEngine.InputSystem;

public class ThirdPersonCameraController : MonoBehaviour
{
    public Transform player;
    public Transform cameraTarget;
    public float rotationSpeed = 1;
    public float distanceFromTarget = 2;
    public float cameraHeightOffset = 2.5f;
    public Vector2 pitchMinMax = new Vector2(-35, 80); // Adjust the max value to prevent flipping

    private float yaw;
    private float pitch;
    private Vector2 currentMouseDelta = Vector2.zero;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Capture the mouse movement
        currentMouseDelta = Mouse.current.delta.ReadValue();
    }

    void LateUpdate()
    {
        // Horizontal rotation
        yaw += currentMouseDelta.x * rotationSpeed * Time.deltaTime;
        player.rotation = Quaternion.Euler(0, yaw, 0);

        // Vertical rotation
        pitch -= currentMouseDelta.y * rotationSpeed * Time.deltaTime; // Notice we subtract to maintain the orientation
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        // Calculate rotation and position
        Vector3 cameraPositionOffset = new Vector3(0, cameraHeightOffset, -distanceFromTarget);
        
        // Apply rotation to camera
        transform.eulerAngles = new Vector3(-pitch, yaw, 0);
        
        // Apply calculated offset from the rotated angle
        transform.position = player.position + Quaternion.Euler(-pitch, yaw, 0) * cameraPositionOffset;

        // Look at the camera target
        transform.LookAt(cameraTarget.position);
    }
}