using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public Transform player;
    public Transform cameraTarget;
    public float rotationSpeed = 1;
    public float distanceFromTarget = 1;
    public float cameraHeightOffset = 2.5f; // Added this variable for height adjustment

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Rotate Camera with Mouse
        float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
        player.Rotate(0, horizontal, 0);

        float desiredYAngle = player.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredYAngle, 0);
        transform.position = player.position - (rotation * Vector3.forward * distanceFromTarget);
        
        // Adjust the height of the camera based on the cameraHeightOffset
        transform.position = new Vector3(transform.position.x, player.position.y + cameraHeightOffset, transform.position.z);
        
        transform.LookAt(cameraTarget);
    }
}
