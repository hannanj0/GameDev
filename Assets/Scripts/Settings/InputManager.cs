using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }

    public PlayerControls Controls { get; private set; }

    public delegate void KeyRebindComplete(string actionName, string bindingDisplayString, int bindingIndex);
    public event KeyRebindComplete OnKeyRebindComplete;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            Controls = new PlayerControls();
            DontDestroyOnLoad(gameObject);
        }
    }

    public void StartRebind(string actionName, int bindingIndex)
    {
        InputAction action = Controls.FindAction(actionName);
        if (action == null)
        {
            Debug.LogError($"Action '{actionName}' not found.");
            return;
        }
        
        action.Disable();

        var binding = action.bindings[bindingIndex];
        if (binding.isComposite)
        {
            // Handle composite binding
            // Assume we start with the first part of the composite
            StartCompositeRebind(action, bindingIndex);
        }
        else if (binding.isPartOfComposite)
        {
            // Handle part of composite binding
            StartPartRebind(action, bindingIndex);
        }
        else
        {
            // Handle non-composite binding
            StartNonCompositeRebind(action, bindingIndex);
        }
    }

    private void StartCompositeRebind(InputAction action, int compositeIndex)
    {
        // Start rebinding for each part of the composite
        for (int i = 1; i < action.bindings.Count; i++)
        {
            if (action.bindings[compositeIndex + i].isPartOfComposite)
            {
                StartPartRebind(action, compositeIndex + i);
                break; // Remove this if you want to start rebinding for all parts at once
            }
        }

    }

    private void StartPartRebind(InputAction action, int partBindingIndex)
    {
        action.PerformInteractiveRebinding(partBindingIndex)
            // Your rebind settings here
            .OnComplete(operation => FinishRebind(operation, action.name, partBindingIndex))
            .Start();
    }

    private void StartNonCompositeRebind(InputAction action, int bindingIndex)
    {
        // Make sure the action is disabled before starting the rebinding
        action.Disable();

        action.PerformInteractiveRebinding(bindingIndex)
            // Your rebind settings here
            .OnComplete(operation => FinishRebind(operation, action.name, bindingIndex))
            .Start();
    }

    private void FinishRebind(InputActionRebindingExtensions.RebindingOperation operation, string actionName, int bindingIndex)
    {
        // Get the new binding path
        string newBindingPath = operation.selectedControl.path;

        // Apply the binding override to the action
        operation.action.ApplyBindingOverride(bindingIndex, newBindingPath);

        string newBinding = operation.action.bindings[bindingIndex].ToDisplayString();

        // Re-enable the action after rebinding
        operation.action.Enable();

        OnKeyRebindComplete?.Invoke(actionName, newBinding, bindingIndex);

        operation.Dispose();
    }

    public string GetCurrentBinding(string actionName, int bindingIndex)
    {
        InputAction action = Controls.FindAction(actionName);
        if (action != null)
        {
            return action.bindings[bindingIndex].ToDisplayString();
        }
        else
        {
            Debug.LogError($"Action '{actionName}' not found.");
            return string.Empty;
        }
    }



}

