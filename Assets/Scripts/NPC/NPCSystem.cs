using UnityEngine;
using UnityEngine.InputSystem;

public class NPCSystem : MonoBehaviour
{
    bool playerDetection = false;

    void OnEnable()
    {
        // Subscribe to the 'performed' event for the Interact action
        InputManager.Instance.Controls.Gameplay.Interact.performed += OnInteractPerformed;
        InputManager.Instance.Controls.Gameplay.Interact.Enable();
    }

    void OnDisable()
    {
        // Unsubscribe from the 'performed' event to prevent memory leaks
        InputManager.Instance.Controls.Gameplay.Interact.performed -= OnInteractPerformed;
        InputManager.Instance.Controls.Gameplay.Interact.Disable();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetection = true;
            Debug.Log("Player is near the NPC.");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDetection = false;
            Debug.Log("Player is no longer near the NPC.");
        }
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        // Check if the player is detected and the Interact action is triggered
        if (playerDetection)
        {
            InitiateConversation();
        }
    }

    void InitiateConversation()
    {
        // Add your conversation logic here
        Debug.Log("Starting conversation with the NPC.");
    }
}