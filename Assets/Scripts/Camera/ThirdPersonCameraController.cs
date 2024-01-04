using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class ThirdPersonCameraController : MonoBehaviour
{
    public Slider sensitivitySlider;
    public Transform player;
    public Transform cameraTarget;

    public PlayerInteractions playerInteractions;
    public PauseMenu pauseMenu;

    public float sensitivity = 5.0f;
    public float distanceFromTarget = 2;
    public float cameraHeightOffset = 2.5f;
    public Vector2 pitchMinMax = new Vector2(-35, 80);

    public TMP_Text compassText;

    private float yaw;
    private float pitch;
    private Vector2 currentMouseDelta = Vector2.zero;

    // Add this variable to control the camera logic
    private bool enableCameraLogic = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (compassText != null)
        {
            compassText.text = "";
        }

        StartCoroutine(EnableScriptAfterDelay());
    }

    IEnumerator EnableScriptAfterDelay()
    {
        yield return new WaitForSeconds(14f);
        enableCameraLogic = true; // Enable the camera logic
    }

    void Update()
    {
        currentMouseDelta = Mouse.current.delta.ReadValue();
    }

    void LateUpdate()
    {
        if (enableCameraLogic && !pauseMenu.GameIsPaused() && !playerInteractions.CraftingMenuOpen())
        {
            // Horizontal rotation
            yaw += currentMouseDelta.x * sensitivity * Time.deltaTime;
            player.rotation = Quaternion.Euler(0, yaw, 0);

            // Vertical rotation
            pitch += currentMouseDelta.y * sensitivity * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

            // Calculate rotation and position
            Vector3 cameraPositionOffset = new Vector3(0, cameraHeightOffset, -distanceFromTarget);

            // Apply rotation to camera
            transform.eulerAngles = new Vector3(-pitch, yaw, 0);

            // Apply calculated offset from the rotated angle
            transform.position = player.position + Quaternion.Euler(-pitch, yaw, 0) * cameraPositionOffset;

            // Look at the camera target
            transform.LookAt(cameraTarget.position);

            // Update compass text
            UpdateCompassText();
        }
    }

    void UpdateCompassText()
    {
        if (compassText != null)
        {
            float compassDirection = (yaw + 360) % 360;
            string cardinalDirection = GetCardinalDirection(compassDirection);
            compassText.text = cardinalDirection;
        }
    }

    string GetCardinalDirection(float angle)
    {
        if (angle >= 337.5f || angle < 22.5f)
            return "N";
        if (angle >= 22.5f && angle < 67.5f)
            return "NE";
        if (angle >= 67.5f && angle < 112.5f)
            return "E";
        if (angle >= 112.5f && angle < 157.5f)
            return "SE";
        if (angle >= 157.5f && angle < 202.5f)
            return "S";
        if (angle >= 202.5f && angle < 247.5f)
            return "SW";
        if (angle >= 247.5f && angle < 292.5f)
            return "W";
        if (angle >= 292.5f && angle < 337.5f)
            return "NW";

        return "";
    }

    public void ChangeSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
    }
}
