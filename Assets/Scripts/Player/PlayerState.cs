using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour
{
    public Animator fadeScene;

    private Vector3 spawnPosition = new Vector3(300f, 4f, 315f);

    public static PlayerState Instance { get; set; }

    private int totalGameBosses;
    private List<string> bossesKilled = new List<string>();

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

    public void SetCurrentHealth(float currentHealth) { currentHealth = this.currentHealth; }
    public void SetMaxHealth(float maxHealth) { maxHealth = this.maxHealth; }
    public void SetCurrentHunger(float currentHunger) { currentHunger = this.currentHunger; }
    public void SetMaxHunger(float maxHunger) { maxHunger = this.maxHunger; }
    public void SetBossesKilled(List<string> bossesKilled) { bossesKilled = this.bossesKilled; }

    public int BossesKilled() { return bossesKilled.Count; }

    public void UpdateSpawnPosition(Vector3 newPosition) { 
        spawnPosition = newPosition;
    }

    public void ResetSpawnPosition() { spawnPosition = new Vector3(300f, 4f, 315f); }

    public void BossKilled(string bossName)
    {
        Debug.Log(bossName);
        bossesKilled.Add(bossName);

        if (BossesKilled() == totalGameBosses)
        {
            Invoke("WinGame", 1.0f);
        }
    }

    public void WinGame()
    {
        StartCoroutine(LoadWinMenu());
    }

    IEnumerator LoadWinMenu()
    {
        yield return new WaitForSeconds(1.5f);

        fadeScene.SetTrigger("FadeOut");

        yield return new WaitForSeconds(1.5f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(3);
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
        playerBody.transform.position = spawnPosition;
        LoadSpawn();;
        totalGameBosses = 2;
        attackDamage = 40.0f;
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        healthDecreaseTimer = healthDecreaseInterval;
    }

    public void LoadSpawn()
    {
        if (PlayerPrefs.HasKey("SpawnPositionX"))
        {
            float spawnX = PlayerPrefs.GetFloat("SpawnPositionX");
            float spawnZ = PlayerPrefs.GetFloat("SpawnPositionZ");
            playerBody.transform.position = new Vector3(spawnX, 0.5f, spawnZ);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // each frame of distance travelled, it will increase by vector3 distance
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;


        if (currentHunger > 0 && distanceTravelled >= 14)
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
