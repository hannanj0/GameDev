using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private WeaponRotation weaponRotation; // Player rotates their weapon to attack.

    
    void Start()
    {
        // Initialise variables.
        canBeAttacked = true;

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

    /// <summary>
    /// Handles trigger colliders - items and enemies.
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
