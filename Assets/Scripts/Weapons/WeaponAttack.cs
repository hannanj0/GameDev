using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

/// <summary>
/// This WeaponAttack script controls the player's attack, deals damage to the enemy and makes the enemy flash red.
/// </summary>
public class WeaponAttack : MonoBehaviour
{
    private bool playerAttacked; // Track whether the player was previously attacking.
    private MeshRenderer enemyMeshRenderer; // Enemy's mesh renderer to make the enemy flash red.
    private Color enemyColor; // Red colour to flash enemy material - visual feedback for attacks.
    float flashDuration = 0.1f; // Red colour flashes for this duration.

    public PlayerState playerState; // Use player state script to read player's current damage.
    public WeaponRotation weaponRotation; // Use weapon rotation script to initiate the attack (rotation of weapon).

    /// <summary>
    /// Set playerAttacked to false when script initialises.
    /// </summary>
    void Start()
    {
        playerAttacked = false;
    }

    /// <summary>
    /// As script loads, get references to objects and scripts.
    /// </summary>
    void Awake()
    {
        // Player and SwordPivot object refereces. 
        GameObject player = GameObject.Find("PlayerAsset");
        GameObject swordPivot = GameObject.Find("SwordPivot");

        // Player stats and weapon rotation script references.
        playerState = player.GetComponent<PlayerState>();
        weaponRotation = swordPivot.GetComponent<WeaponRotation>();
    }

    /// <summary>
    /// Trigger collider events to deal damage to enemies and provide visual feedback through enemy health bar UI and flashing red enemy.
    /// </summary>
    /// <param name="other"> Other entity colliding with the player's sword. </param>
    private void OnTriggerEnter(Collider other)
    {
        // Player's weapon must collide with an enemy. Damaging an enemy can only take place if an attack is initiated (meaning their weapon is rotating).
        // Before user input to attack, an attack must not have previously been in effect, and they must be off cooldown to attack again.
        if (this.gameObject.CompareTag("Weapon") && other.gameObject.CompareTag("Enemy") && weaponRotation.IsAttacking() && !playerAttacked)
        {
            // Deal damage to the enemy.
            EnemyState enemy = other.gameObject.GetComponent<EnemyState>();
            enemy.TakeDamage(playerState.AttackDamage());

            playerAttacked = true;

            // Flash enemy red for damage indication.
            enemyMeshRenderer = other.gameObject.GetComponent<MeshRenderer>();
            enemyColor = enemyMeshRenderer.materials[0].color;

            // Update enemy health bar UI.
            EnemyHealthBar enemyHealthBar = other.transform.Find("HealthBarContainer/HealthBar").GetComponent<EnemyHealthBar>();
            enemyHealthBar.UpdateHealthBar(enemy.Health(), enemy.MaxHealth());
            FlashEnemyStart();

            // Destroy enemy when their health reaches 0 or less.
            if (enemy.Health() <= 0)
            {
                // If final boss is killed, player wins - load win menu.
                if (enemy.IsBoss())
                {
                    playerState.BossKilled();
                }
                other.gameObject.SetActive(false);
                playerAttacked = false;
            }
        }
    }

    /// <summary>
    /// When the player's weapon initiated an attack (playerAttacked) and stops colliding with the enemy, the player is no longer attacking.
    /// </summary>
    /// <param name="other"> Other entity the player's weapon stops colliding with. </param>
    private void OnTriggerExit(Collider other)
    {
        if (this.gameObject.CompareTag("Weapon") && other.gameObject.CompareTag("Enemy") && playerAttacked)
        {
            // Once false (after finishing an attack), allow the player to attack again.
            playerAttacked = false;
        }
    }

    /// <summary>
    /// Starts plashing the enemy's material colour to red for flashDuration.
    /// </summary>
    void FlashEnemyStart()
    {
        enemyMeshRenderer.materials[0].color = Color.red;
        Invoke("FlashEnemyFinish", flashDuration);
    }


    /// <summary>
    /// Stops flashing the red colour, sets the enemy's material back to its original colour.
    /// </summary>
    void FlashEnemyFinish()
    {
        enemyMeshRenderer.materials[0].color = enemyColor;
    }
}