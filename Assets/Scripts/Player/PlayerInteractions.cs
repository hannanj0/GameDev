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
    private bool canBeAttacked; // Cooldown to check if the player can be attacked.
    private float enemyAttackCooldown = 0.0f; // Counter to see how often the enemy can attack the player.
    private float enemyAttackCooldownDuration = 2.0f; // Duration the enemy has to wait before attacking again.

    private PlayerState playerState; // Player stats inside script.
    private EnemyState enemyState;
    private Inventory inventory; // Player manages their inventory.
    private InputAction useItemAction;
    private InputAction openCMenu;
    private WeaponRotation weaponRotation; // Player rotates their weapon to attack.
    private ItemDescription background;
    private Canvas craftingTable;


    void Start()
    {
        // Initialise variables.
        canBeAttacked = true;

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

        openCMenu = new InputAction("OpenCraftingMenu", binding:"<Keyboard>/tab");
        openCMenu.performed += OpenCraftingMenu;
        openCMenu.Enable();
    }

    /// <summary>
    /// Update the boolean to indicate whether the player can be attacked.
    /// If the duration is reached, they can be attacked. After, being attacked, the cooldown applies again.
    /// </summary>
    void Update()
    {
        if (!canBeAttacked)
        {
            enemyAttackCooldown += Time.deltaTime;
            if (enemyAttackCooldown >= enemyAttackCooldownDuration)
            {
                playerState.currentHealth -= enemyState.AttackDamage();
                canBeAttacked = true;
                enemyAttackCooldown = 0.0f;
            }
        }
    }

    private void UseItem(InputAction.CallbackContext context)
    {
        Debug.Log(context.control.name);
        if (!inventory.hotbarSlots[inventory.currentSlot].isEmpty)
        {
            if (inventory.hotbarSlots[inventory.currentSlot].assignedItem.consumable)
            {
                inventory.hotbarSlots[inventory.currentSlot].assignedItem.Use(playerState);
                inventory.hotbarSlots[inventory.currentSlot].RemoveItem();
            }
        }
    }
    private void OpenCraftingMenu(InputAction.CallbackContext context)
    {
        craftingTable.enabled = !craftingTable.enabled;
        if (craftingTable.enabled)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
        else
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
    }

    /// <summary>
    /// Registers a collision with the enemy if the player can be attacked. Reduce player health.
    /// </summary>
    /// <param name="other"> Other entity colliding with the player. </param>
    private void OnCollisionEnter(Collision other)
    {
        // Take damage from the enemy when the player can be attacked again.
        if (other.gameObject.CompareTag("Enemy") && canBeAttacked)
        {
            enemyState = other.gameObject.GetComponent<EnemyState>();
            playerState.currentHealth -= enemyState.AttackDamage();
            canBeAttacked = false;
        }
        // The player cannot be attacked for the AttackCooldownDuration since they just got attacked.
    }

    /// <summary>
    /// Register the exit of the collision with an enemy. They can be attacked again after no longer colliding.
    /// </summary>
    /// <param name="other"> Other entity colliding with the player. </param>
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            canBeAttacked = true;
            enemyAttackCooldown = 0.0f;
        }
    }

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
}
