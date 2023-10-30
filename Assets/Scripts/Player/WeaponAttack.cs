using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeaponAttack : MonoBehaviour
{
    private bool playerAttacked;
    private SkinnedMeshRenderer enemyMeshRenderer;
    private Color enemyColor;
    float flashDuration = 0.1f;

    public PlayerState playerState;
    public WeaponRotation weaponRotation;


    void Start()
    {
        playerAttacked = false;
    }

    void Awake()
    {
        GameObject player = GameObject.Find("Player");
        GameObject swordPivot = GameObject.Find("SwordPivot");

        // Get access to player attack damage.
        playerState = player.GetComponent<PlayerState>();

        // Get access to sword animation to time player attacks and damage dealt.
        weaponRotation = swordPivot.GetComponent<WeaponRotation>();
    }

    // Trigger events dealing damage to enemies.
    private void OnTriggerEnter(Collider other)
    {
        // Player attacks an enemy with their weapon. playerAttacked boolean to negate multiple trigger events.
        if (this.gameObject.CompareTag("Weapon") && other.gameObject.CompareTag("Enemy") && weaponRotation.isAttacking && !playerAttacked)
        {
            // Deal damage to enemy.
            EnemyState enemy = other.gameObject.GetComponent<EnemyState>();
            enemy.health -= playerState.attackDamage;
            playerAttacked = true;
            Debug.Log("damaged enemy");

            // Flash enemy red for damage indication. https://www.youtube.com/watch?v=3aWgstSctMw
            enemyMeshRenderer = other.transform.Find("Meshes/Body").GetComponent<SkinnedMeshRenderer>();
            enemyColor = enemyMeshRenderer.materials[0].color;
            FlashEnemyStart();

            // Update enemy health bar.
            EnemyHealthBar enemyHealthBar = other.transform.Find("HealthBarContainer/HealthBar").GetComponent<EnemyHealthBar>();
            enemyHealthBar.UpdateHealthBar(enemy.health, enemy.maxHealth);


            // Destroy enemy when their health reaches 0 or less.
            if (enemy.health <= 0)
            {
                // If final boss is killed, player wins - load win menu.
                if (enemy.isBoss)
                {
                    Time.timeScale = 0;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    SceneManager.LoadScene("WinGame");
                }
                other.gameObject.SetActive(false);
            }
        }
    }

    // Manage trigger events with player weapon. Fixes issue of multiple triggers.
    private void OnTriggerExit(Collider other)
    {
        if (this.gameObject.CompareTag("Weapon") && other.gameObject.CompareTag("Enemy") && playerAttacked)
        {
            // Once false (after finishing an attack), allow the player to attack again.
            playerAttacked = false;
        }
    }

    void FlashEnemyStart()
    {
        enemyMeshRenderer.materials[0].color = Color.red;
        Invoke("FlashEnemyFinish", flashDuration);
    }

    void FlashEnemyFinish() 
    {
        enemyMeshRenderer.materials[0].color = enemyColor;
    }
}