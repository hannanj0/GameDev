using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponAttack : MonoBehaviour
{
    private Animator animator;
    private PlayerControls playerControls;
    private bool playerAttacked; // Track whether the player was previously attacking.
    private Renderer enemyRenderer; // Enemy's mesh renderer to make the enemy flash red.
    private Color enemyColor; // Red colour to flash enemy material - visual feedback for attacks.
    private float flashDuration = 0.1f; // Red colour flashes for this duration.

    public PlayerState playerState; // Use player state script to read player's current damage.

    void Awake()
    {
        // Initialize the PlayerControls
        playerControls = InputManager.Instance.Controls;

        // Get references to the player's Animator, PlayerState
        GameObject player = GameObject.FindWithTag("Player");
        animator = player.GetComponent<Animator>();
        playerState = player.GetComponent<PlayerState>();
    }

    void Update()
    {
        // Assuming the Melee Attack state is on layer 0, change if it's on a different layer
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Melee Attack") &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            animator.ResetTrigger("isMeleeAttack");
        }
    }

    private void OnEnable()
    {
        // Enable the Gameplay action map
        playerControls.Gameplay.Enable();

        // Subscribe to the 'performed' event for the Attack action
        playerControls.Gameplay.Attack.performed += OnAttackPerformed;
    }

    private void OnDisable()
    {
        // Disable the Gameplay action map
        playerControls.Gameplay.Disable();

        // Unsubscribe from the 'performed' event to prevent memory leaks
        playerControls.Gameplay.Attack.performed -= OnAttackPerformed;
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        // Trigger the attack animation and set the player as having attacked
        TriggerAttackAnimation();
        playerAttacked = true;
    }

    private void TriggerAttackAnimation()
    {
        if (animator != null)
        {
            // Set the isMeleeAttack trigger to start the animation
            animator.SetTrigger("isMeleeAttack");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && IsPlayerAttacking())
        {
            ApplyDamageToEnemy(other);
            playerAttacked = false;
        }
    }

    private bool IsPlayerAttacking()
    {
        // Check if the player is currently in the attack animation
        return playerAttacked && animator.GetCurrentAnimatorStateInfo(0).IsName("Melee Attack");
    }

    private void ApplyDamageToEnemy(Collider enemyCollider)
    {
        EnemyState enemy = enemyCollider.gameObject.GetComponent<EnemyState>();
        if (enemy != null)
        {
            enemy.TakeDamage(playerState.AttackDamage());
            Debug.Log(playerState.AttackDamage());

            if (enemyCollider.gameObject.name == "Bear_4")
            {
                enemyRenderer = enemyCollider.transform.Find("Meshes/Body").GetComponent<SkinnedMeshRenderer>();
            }
            else
            {
                enemyRenderer = enemyCollider.gameObject.GetComponent<MeshRenderer>();
            }

            enemyColor = enemyRenderer.material.color;
            FlashEnemyStart();

            EnemyHealthBar enemyHealthBar = enemyCollider.transform.Find("HealthBarContainer/HealthBar").GetComponent<EnemyHealthBar>();
            enemyHealthBar.UpdateHealthBar(enemy.Health(), enemy.MaxHealth());

            if (enemy.Health() <= 0)
            {
                if (enemy.IsBoss())
                {
                    string bossName = enemyCollider.gameObject.GetComponent<BossEnemyState>().BossName();
                    playerState.BossKilled(bossName);
                }
                enemyCollider.gameObject.SetActive(false);
            }
        }
    }

    void FlashEnemyStart()
    {
        enemyRenderer.material.color = Color.red;
        Invoke(nameof(FlashEnemyFinish), flashDuration);
    }

    void FlashEnemyFinish()
    {
        enemyRenderer.material.color = enemyColor;
    }

    public void ResetMeleeAttack()
    {
        animator.ResetTrigger("isMeleeAttack");
    }
}
