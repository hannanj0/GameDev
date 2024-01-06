using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.Playables;

/// <summary>
/// This script is used for the player camera itself, which is attached to the main camera, and used throughout the game, with the exception of cutscenes, where it gets temporarily disabled
/// </summary>
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
    public PlayerControls playerControls;

    public TMP_Text compassText;

    private float yaw; // This is the horizontal rotation of the object
    private float pitch; // This is the vertical rotation of the object
    private Vector2 currentMouseDelta = Vector2.zero;

    // Add this variable to control the camera logic
    private bool enableCameraLogic = false;

    private PlayableDirector playableDirector; // Add this variable

    void Start()
    {
        playerControls = new PlayerControls();
        playerControls.Gameplay.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (compassText != null)
        {
            compassText.text = "";
        }

        StartCoroutine(EnableScriptAfterDelay());

        // Find the PlayableDirector component
        playableDirector = GetComponent<PlayableDirector>();
        if (playableDirector != null)
        {
            playableDirector.stopped += OnCutsceneFinished;
        }
    }

    void OnDestroy()
    {
        playerControls.Gameplay.Disable();
    }

    IEnumerator EnableScriptAfterDelay()
    {
        yield return new WaitForSeconds(14f);
        enableCameraLogic = true; // Enable the camera logic
    }

    void Update()
    {
        // Check if gamepad is connected and use its right stick input
        if (Gamepad.current != null)
        {
            Vector2 gamepadInput = playerControls.Gameplay.CameraControl.ReadValue<Vector2>();
            currentMouseDelta = new Vector2(gamepadInput.x, -gamepadInput.y); // Inverting Y for gamepad to match typical gamepad controls
        }
        else
        {
            // Use mouse input if no gamepad is detected
            currentMouseDelta = Mouse.current.delta.ReadValue();
        }

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

    /// <summary>
    /// The two below is used for the compass in-game, getting the Cardinal Direction based on 360 degrees, with if-else statements
    /// </summary>

    /// <summary>
    /// This is used to update the text displaying the compass direction, and gets the cardinal direction based on the compass direction
    /// </summary>
    void UpdateCompassText()
    {
        if (compassText != null)
        {
            float compassDirection = (yaw + 360) % 360; // makes sure the compass direction is between 0 and 360
            string cardinalDirection = GetCardinalDirection(compassDirection);
            compassText.text = cardinalDirection;
        }
    }

    /// <summary>
    /// This is used to convert the angle to a cardinal direction, using if-else statements to deduce what it should display
    /// </summary>
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
    
    /// <summary>
    /// This is the change the sensitivity of the compass rotation
    /// </summary>
    public void ChangeSensitivity(float newSensitivity)
    {
        sensitivity = newSensitivity;
    }

    // Add this method to enable or disable the camera logic
    public void EnableCameraLogic(bool enable)
    {
        enableCameraLogic = enable;
    }

    // Add this method to handle cutscene completion event
    private void OnCutsceneFinished(PlayableDirector director)
    {
        // Enable the camera logic after the cutscene finishes
        StartCoroutine(EnableCameraAfterDelay(15f));
    }

    IEnumerator EnableCameraAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        EnableCameraLogic(true);
    }
}
