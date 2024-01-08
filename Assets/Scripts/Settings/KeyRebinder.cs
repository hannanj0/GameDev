using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class KeyRebinder : MonoBehaviour
{
    [SerializeField] private InputActionReference actionReference; 
    [SerializeField] private TextMeshProUGUI assignedKeyText; 
    [SerializeField] private int bindingIndex;

    public void StartRebindingProcess()
    {
        // Let the user know to press a new key
        assignedKeyText.text = $"Press a new key for '{actionReference.action.name}' action";

        // Start the rebinding process using InputManager
        InputManager.Instance.StartRebind(actionReference.action.name, bindingIndex);
    }

    private void OnEnable()
    {
        InputManager.Instance.OnKeyRebindComplete += HandleRebindComplete;

        if (actionReference != null && assignedKeyText != null)
        {
            string currentBinding = InputManager.Instance.GetCurrentBinding(actionReference.action.name, bindingIndex);
            // assignedKeyText.text = $"current: '{currentBinding}'";
            assignedKeyText.text = currentBinding;
        }
    }




    private void OnDisable()
    {
        // Unsubscribe from the rebind event
        InputManager.Instance.OnKeyRebindComplete -= HandleRebindComplete;
    }

    private void HandleRebindComplete(string actionName, string newBinding, int bindingIndex)
    {
        // Check if the completed rebind action is the one we're referencing
        if (actionReference != null && actionName == actionReference.action.name && this.bindingIndex == bindingIndex)
        {
            // Update the UI with the new binding
            assignedKeyText.text = newBinding;
        }
    }


}

