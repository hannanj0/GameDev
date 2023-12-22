using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponAttack : MonoBehaviour
{
    private Animator animator;
    private PlayerControls playerControls;
    private bool playerAttacked; // Track whether the player was previously attacking.
    private MeshRenderer enemyMeshRenderer; // Enemy's mesh renderer to make the enemy flash red.
    private Color enemyColor; // Red colour to flash enemy material - visual feedback for attacks.
    private float flashDuration = 0.1f; // Red colour flashes for this duration.

    public PlayerState playerState; // Use player state script to read player's current damage.

    void Awake()
    {
        // Initialize the PlayerControls
        playerControls = new PlayerControls();

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
        if (other.gameObject.CompareTag("Enemy") && playerAttacked)
        {
            EnemyState enemy = other.gameObject.GetComponent<EnemyState>();
            enemy.TakeDamage(playerState.AttackDamage());
            Debug.Log(playerState.AttackDamage());

            enemyMeshRenderer = other.gameObject.GetComponent<MeshRenderer>();
            enemyColor = enemyMeshRenderer.material.color;

            EnemyHealthBar enemyHealthBar = other.transform.Find("HealthBarContainer/HealthBar").GetComponent<EnemyHealthBar>();
            enemyHealthBar.UpdateHealthBar(enemy.Health(), enemy.MaxHealth());

            FlashEnemyStart();

            if (enemy.Health() <= 0)
            {
                if (enemy.IsBoss())
                {
                    string bossName = other.gameObject.GetComponent<BossEnemyState>().BossName();
                    playerState.BossKilled(bossName);
                }
                other.gameObject.SetActive(false);
            }
            // Reset the attack state to allow for another attack
            playerAttacked = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && playerAttacked)
        {
            playerAttacked = false;
        }
    }

    void FlashEnemyStart()
    {
        enemyMeshRenderer.material.color = Color.red;
        Invoke(nameof(FlashEnemyFinish), flashDuration);
    }

    public void ResetMeleeAttack()
    {
        animator.ResetTrigger("isMeleeAttack");
    }


    void FlashEnemyFinish()
    {
        enemyMeshRenderer.material.color = enemyColor;
    }
}
