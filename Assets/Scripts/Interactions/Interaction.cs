using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets.Interactions;

/// <summary>
/// This is the script attached to the player, which uses the new input system so that when pressing "E", it triggers the interaction with the NPC
/// </summary>
public class Interaction : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer; // The NPC has the layer interactable so that this works
    private PlayerInput _playerInput;
    private Transform _transform;

    private void Awake() 
    {
        _transform = transform;
        _playerInput = GetComponent<PlayerInput>(); // Gets PlayerInput component
    }

    private void OnEnable() 
    {
        _playerInput.actions["Interact"].performed += DoInteract; // Using new input system
    }

    private void OnDisable() 
    {
        _playerInput.actions["Interact"].performed -= DoInteract; // Using new input system
    }

    /// <summary>
    /// When the interact action is done, it will check whether the player is facing the NPC using raycasting and calls the Interact method on the InteractableObject
    /// </summary>
    private void DoInteract(InputAction.CallbackContext callbackContext) 
    {
        float interactDistance = 10f;

        if (!Physics.Raycast(_transform.position + (Vector3.up * 0.3f) + (_transform.forward * 0.2f), _transform.forward, out var hit, interactDistance, interactableLayer)) return;

        if (!hit.transform.TryGetComponent(out InteractableObject interactable)) return;


        interactable.Interact();
        Debug.Log(("Interact"));
    }
}
