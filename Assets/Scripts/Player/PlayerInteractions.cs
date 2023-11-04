using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The PlayerInteractions script is a Unity MonoBehaviour class that handles interactions and behaviors for a player character in the game.
/// It manages player input, item usage, collisions with enemies and items, and more.
/// </summary>
public class PlayerInteractions : MonoBehaviour
{
    private bool offCooldown;
    private float enemyCollisionCooldown = 0.0f;
    private float enemyCollisionCooldownDuration = 2.0f;

    private PlayerState playerState;
    private Inventory inventory;
    private InputAction useItemAction;
    private WeaponRotation weaponRotation;

    
    void Start()
    {
        // Initialise variables.
        offCooldown = true;

        Transform inventoryPlayer = transform.Find("Inventory");
        playerState = GetComponent<PlayerState>();
        weaponRotation = transform.Find("WeaponSlot/SwordPivot").GetComponent<WeaponRotation>();

        if (inventoryPlayer != null) { inventory = inventoryPlayer.GetComponent<Inventory>(); }

        if (inventoryPlayer == null) { Debug.LogError("No inventory"); }

        // Set up input action for "UseItem"
        useItemAction = new InputAction("UseItem", binding: "<Keyboard>/f");
        useItemAction.performed += UseItem;
        useItemAction.Enable();
    }

    /// <summary>
    /// Update the boolean to indicate whether the player can take damage.
    /// The player cannot be attacked for 2 seconds after being attacked.
    /// </summary>
    void Update()
    {
        if (!offCooldown)
        {
            enemyCollisionCooldown += Time.deltaTime;
            if (enemyCollisionCooldown >= enemyCollisionCooldownDuration)
            {
                offCooldown = true;
                enemyCollisionCooldown = 0.0f;
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

    /// <summary>
    /// Handles trigger colliders - items and enemies.
    /// </summary>
    /// <param name="other"> Other entity colliding with the player. </param>
    private void OnTriggerEnter(Collider other)
    {
        //
        if (other.gameObject.CompareTag("Item"))
        {
            GameItem I = other.GetComponent<GameItem>();
            Debug.Log("Item Picked Up");
            inventory.Add(I.item);
            other.gameObject.SetActive(false);
        }

        // Take damage from the enemy when player's protection runs out (every 2 seconds).
        if (other.gameObject.CompareTag("Enemy") && offCooldown)
        {
            Debug.Log("player collided");
            EnemyState enemy = other.gameObject.GetComponent<EnemyState>();
            playerState.currentHealth -= enemy.AttackDamage();
        }
        offCooldown = false;
        enemyCollisionCooldown = 0.0f;
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
