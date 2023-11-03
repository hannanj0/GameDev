using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public Transform player;
    public Transform cameraTarget;

    public PauseMenu pauseMenu;

    public float rotationSpeed = 1;
    public float distanceFromTarget = 2;
    public float cameraHeightOffset = 2.5f;
    public Vector2 pitchMinMax = new Vector2(-35, 80); // Adjust the max value to prevent flipping

    private float yaw;
    private float pitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (!pauseMenu.GameIsPaused())
        {
            // Horizontal rotation
            yaw += Input.GetAxis("Mouse X") * rotationSpeed;
            player.rotation = Quaternion.Euler(0, yaw, 0);

            // Vertical rotation
            pitch += Input.GetAxis("Mouse Y") * rotationSpeed;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

            // Calculate rotation and position
            Vector3 cameraPositionOffset = new Vector3(0, cameraHeightOffset, -distanceFromTarget);

            // Apply rotation to camera
            transform.eulerAngles = new Vector3(-pitch, yaw, 0); // Ensure pitch is applied negatively for a correct "look down" orientation

            // Apply calculated offset from the rotated angle
            transform.position = player.position + Quaternion.Euler(-pitch, yaw, 0) * cameraPositionOffset;

            // Look at the camera target
            transform.LookAt(cameraTarget.position);
        }
    }
}
