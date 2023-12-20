using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/// <summary>
/// The PlayerInteractions script manages the player's interactions in the game.
/// It manages item usage, the inventory and taking damage.
/// </summary>
public class PlayerInteractions : MonoBehaviour
{
    public GameObject menuController;

    private bool inContact; // check if the player is in contact with an enemy.
    private bool craftingMenuOpen; // Check if the crafting menu is open.
    private float enemyAttackCooldown = 0.0f; // Counter to see how often the enemy can attack the player.
    private float enemyAttackCooldownDuration = 1.5f; // Duration the enemy has to wait before attacking again.

    private PlayerState playerState; // Player stats inside script.
    private EnemyState enemyState; // EnemyState script to read their damage.
    private Inventory inventory; // Player manages their inventory.
    private InputAction useItemAction;
    private InputAction openCMenu;
    private WeaponRotation weaponRotation; // Player rotates their weapon to attack.
    private ItemDescription background;
    private Canvas craftingTable;

    public bool CraftingMenuOpen() {  return craftingMenuOpen; }

    void Start()
    {
        // Initialise variables.
        inContact = false;
        craftingMenuOpen = false;

        Transform inventoryPlayer = transform.Find("Inventory");
        inventory = inventoryPlayer.GetComponent<Inventory>();

        weaponRotation = transform.Find("WeaponSlot/SwordPivot").GetComponent<WeaponRotation>();

        playerState = GetComponent<PlayerState>();

        Transform backgroundChild = transform.Find("ItemInformation/BackgroundColour");
        background = backgroundChild.GetComponent<ItemDescription>();

        Transform craftingPlayer = transform.Find("Crafting");
        craftingTable = craftingPlayer.GetComponent<Canvas>();
        // Set up input action for "UseItem"
        useItemAction = new InputAction("UseItem", binding: "<Keyboard>/f");
        useItemAction.performed += UseItem;
        useItemAction.Enable();
        // Set up input action for "OpenCraftingMenu"
        openCMenu = new InputAction("OpenCraftingMenu", binding:"<Keyboard>/tab");
        openCMenu.performed += OpenCraftingMenu;
        openCMenu.Enable();
    }

    /// <summary>
    /// Update the timer and keep track of the player taking damage.
    /// The player is attacked every enemyAttackCooldownDuration if they are in contact with an enemy.
    /// </summary>
    void Update()
    {
            enemyAttackCooldown += Time.deltaTime;
            if (enemyAttackCooldown >= enemyAttackCooldownDuration)
            {
                if (inContact && enemyState.Health() > 0)
                {
                    playerState.TakeDamage(enemyState.AttackDamage());
                    enemyAttackCooldown = 0.0f;
            }
            }
    }
    /// <summary>
    /// Uses an item
    /// If the slot is not empty uses the item and then removes it
    /// </summary>
    private void UseItem(InputAction.CallbackContext context)
    {
        if (!inventory.hotbarSlots[inventory.currentSlot].isEmpty)
        {
            if (inventory.hotbarSlots[inventory.currentSlot].assignedItem.consumable)
            {
                inventory.hotbarSlots[inventory.currentSlot].assignedItem.Use(playerState);
                inventory.hotbarSlots[inventory.currentSlot].RemoveItem();
            }
        }
    }
    /// <summary>
    /// Opens the crafting menu
    /// When the butten assigned to input action is pressed the canvas containing the crafting menu is enabled
    /// The cursor is made visiable and not locked and the game is paused
    /// When the button is pressed again the canvas becomes disabled and the cursor is locked and made invisible and the game resumes running
    /// </summary>
    private void OpenCraftingMenu(InputAction.CallbackContext context)
    {
        craftingTable.enabled = !craftingTable.enabled;
        if (craftingTable.enabled)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            craftingMenuOpen = true;
            Time.timeScale = 0.0f;
        }
        else
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            craftingMenuOpen = false;
            Time.timeScale = 1.0f;
        }
    }

    /// <summary>
    /// Registers a collision with the enemy, they are in contact so they can be attacked.
    /// </summary>
    /// <param name="other"> Other entity colliding with the player. </param>
    private void OnCollisionEnter(Collision other)
    {
        // Take damage from the enemy when the player can be attacked again.
        if (other.gameObject.CompareTag("Enemy") && !inContact)
        {
            enemyState = other.gameObject.GetComponent<EnemyState>();
            inContact = true;
        }
        // The player cannot be attacked for the AttackCooldownDuration since they just got attacked.
    }

    /// <summary>
    /// Registers a collision exit with the enemy. They are no longer in contact.
    /// </summary>
    /// <param name="other"> Other entity colliding with the player. </param>
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            inContact = false;
        }
    }
    /// <summary>
    /// Used to pick up an item
    /// If the object has the item tag it is picked up
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        //
        if (other.gameObject.CompareTag("Item"))
        {
            GameItem I = other.GetComponent<GameItem>();
            Debug.Log("Item Picked Up");
            inventory.Add(I.item);
            other.gameObject.SetActive(false);

            background.DisplayDescription(I);
        }

        else if ( other.tag == "Checkpoint")
        {
            playerState.UpdateSpawnPosition(other.transform.position);
            PlayerPrefs.SetFloat("SpawnPosition" + "X", other.transform.position.x);
            PlayerPrefs.SetFloat("SpawnPosition" + "Z", other.transform.position.z);

            PlayerPrefs.Save();
            //reset health and hunger
            // reset health of all enemies
        }
    }

    /// <summary>
    /// Initiate player's attack rotation.
    /// </summary>
    private void OnAttack()
    {
        weaponRotation.BeginAttack();
    }

    private void OnDestroy()
    {
        // Clean up and disable the action when this object is destroyed
        useItemAction.performed -= UseItem;
        useItemAction.Disable();
    }

    private void OnPause()
    {
        menuController.GetComponent<PauseMenu>().OnPause();
    }
}
