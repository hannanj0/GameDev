using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour
{

    public static PlayerState Instance { get; set; }

    private int bossesKilled;
    private int totalGameBosses;

    // Player Health
    public float currentHealth;
    public float maxHealth;


    // Player Hunger
    public float currentHunger;
    public float maxHunger;

    //Player stats
    private float attackDamage;

    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;

    private float healthDecreaseInterval = 5f;
    private float healthDecreaseTimer;

    public int BossesKilled() { return bossesKilled; }

    public void BossKilled()
    {
        bossesKilled += 1;
        if (bossesKilled == totalGameBosses)
        {
            Invoke("WinGame", 1.0f);
        }
    }

    public void WinGame()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("WinGame");
    }

    public float AttackDamage()
    {
        return attackDamage;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void IncreaseDamage(float damage)
    {
        attackDamage += damage;
    }

    // awake looks and checks that it is the only instance in the game, if not, it will destroy it
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        bossesKilled = 0;
        totalGameBosses = 1;
        attackDamage = 40.0f;
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        healthDecreaseTimer = healthDecreaseInterval;
    }

    // Update is called once per frame
    void Update()
    {
        // each frame of distance travelled, it will increase by vector3 distance
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;


        if (currentHunger > 0 && distanceTravelled >= 7)
        {
            distanceTravelled = 0;
            currentHunger -= 1;

            // ensures the hunger bar does not go into negative value
            if (currentHunger < 0)
            {
                currentHunger = 0;
            }
        }

        // the game ends when health reaches 0, this is the loss condition
        if (currentHealth <= 0)
        {
            Time.timeScale = 0;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("GameOver");
        }

        // when the hunger reaches 0, the health will slowly deteriorate
        if (currentHunger == 0)
        {
            healthDecreaseTimer -= Time.deltaTime;
            if (healthDecreaseTimer <= 0)
            {
                currentHealth -= 5;
                healthDecreaseTimer = healthDecreaseInterval;
            }
        }
    }
}
