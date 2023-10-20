using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public Transform player;
    public Transform cameraTarget;
    public float rotationSpeed = 1;
    public float distanceFromTarget = 1;
    
    void Update()
    {
        // Rotate Camera with Mouse
        float horizontal = Input.GetAxis("Mouse X") * rotationSpeed;
        player.Rotate(0, horizontal, 0);

        float desiredYAngle = player.eulerAngles.y;
        Quaternion rotation = Quaternion.Euler(0, desiredYAngle, 0);
        transform.position = player.position - (rotation * Vector3.forward * distanceFromTarget);
        
        transform.position = new Vector3(transform.position.x, player.position.y + 1.5f, transform.position.z);
        
        transform.LookAt(cameraTarget);
    }
}
