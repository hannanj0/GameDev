using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets.Interactions;

public class Interaction : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer;
    private PlayerInput _playerInput;
    private Transform _transform;

    private void Awake() 
    {
        _transform = transform;
        _playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable() 
    {
        _playerInput.actions["Interact"].performed += DoInteract;
    }

    private void OnDisable() 
    {
        _playerInput.actions["Interact"].performed -= DoInteract;
    }

    private void DoInteract(InputAction.CallbackContext callbackContext) 
    {
        if (!Physics.Raycast(_transform.position + (Vector3.up * 0.3f) + (_transform.forward * 0.2f), _transform.forward, out var hit, 1.5f, interactableLayer)) return;

        if (!hit.transform.TryGetComponent(out InteractableObject interactable)) return;

        // Check if there's a conversation attached
        if (interactable.Conversation != null)
        {
            // Start the conversation with a predefined dialogue array
            string[] dialogue = new string[] { "Hello!", "How are you?", "Goodbye!" };
            interactable.Conversation.StartConversation(dialogue);
        }

        interactable.Interact();
        Debug.Log(("Interact"));
    }
}
