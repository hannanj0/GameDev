using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Inventory : MonoBehaviour
{
    public PlayerControls controls;  // Using the generated C# class
    private List<Item> inventory = new List<Item>();
    public HotbarSlot[] hotbarSlots;
    public int currentSlot = 0;
    public Item currentItem;
    /// <summary>
    /// Methods that are used for the input system
    /// </summary>
    private void Awake()
    {
        controls = new PlayerControls();  // Initialize the controls
        controls.Gameplay.HotBarSelect.performed += OnHotbarKey;  // Update reference to Gameplay
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void OnHotbarKey(InputAction.CallbackContext context)
    {
        Debug.Log(context.control.name);
        string key = context.control.name;

        int slotIndex = -1;
        if (int.TryParse(key, out slotIndex))
        {
            SelectSlot(slotIndex - 1);
        }
    }
    /// <summary>
    /// Adds the item to the inventory and the hotbar
    /// </summary>
    public void Add(Item item)
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            HotbarSlot slot = hotbarSlots[i];
            if(slot.isEmpty)
            {
                inventory.Add(item);
                slot.AssignItem(item);
                Debug.Log(inventory);
                break;
            }
        }
    }
    /// <summary>
    /// Used to select a slot and deselect the previous one
    /// </summary>
    void SelectSlot(int newSlot)
    {
        hotbarSlots[currentSlot].Deselect();
        hotbarSlots[newSlot].Select();
        currentSlot = newSlot;
    }
}

