using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PlayerState : MonoBehaviour
{
    PlayerData playerData;
    bool dataPending = false;

    public MainManager mainManager;

    public GameObject menuController;

    public Animator fadeScene;

    public Vector3 spawnPosition = new Vector3(300f, 4f, 315f);

    public static PlayerState Instance { get; set; }

    private int totalGameBosses = 2;
    private List<string> bossesKilled = new List<string>();

    // Player Health
    public float currentHealth;
    public float maxHealth;


    // Player Hunger
    public float currentHunger;
    public float maxHunger;
    public float loseHealthFromHunger = 5f;
    //Player stats
    private float baseDamage = 40f;
    private float extraDamage = 0f;
    float distanceTravelled = 0;
    Vector3 lastPosition;

    public GameObject playerBody;

    private float healthDecreaseInterval = 5f;
    private float healthDecreaseTimer;

    public void SetCurrentHealth(float c) { c = currentHealth; }
    public void SetMaxHealth(float m) { m = maxHealth; }
    public void SetCurrentHunger(float currentHunger) { currentHunger = this.currentHunger; }
    public void SetMaxHunger(float maxHunger) { maxHunger = this.maxHunger; }
    public void SetBossesKilled(List<string> bossesKilled) { bossesKilled = this.bossesKilled; }

    public void SetPlayerSpawn(Vector3 newPosition) { playerBody.transform.position = newPosition; }

    public float CurrentHealth() { return currentHealth; }
    public float MaxHealth() { return maxHealth; }
    public float CurrentHunger() {  return currentHunger; }
    public float MaxHunger() { return maxHunger; }
    public float BaseDamage() {  return baseDamage; }
    public float ExtraDamage() { return extraDamage; }
    public float AttackDamage() { return baseDamage + extraDamage; }
    public List<string> BossesKilled() {  return bossesKilled; }
    public float SpawnX() { return spawnPosition.x; }
    public float SpawnY() {  return spawnPosition.y; }
    public float SpawnZ() {  return spawnPosition.z; }
    public int TotalGameBosses() { return totalGameBosses; }


    public int BossesKilledCount() { return bossesKilled.Count; }

    public void ResetSpawnPosition() { spawnPosition = new Vector3(300f, 4f, 315f); }

    public void BossKilled(string bossName)
    {
        bossesKilled.Add(bossName);

        if (BossesKilledCount() == totalGameBosses)
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

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }

    public void IncreaseDamage(float damage)
    {
        extraDamage += damage;
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
        if (GameManager.Instance != null && GameManager.Instance.loadGameRequest)
        {
            dataPending = true;
            playerData = mainManager.LoadPlayerData();

            healthDecreaseTimer = healthDecreaseInterval;
            this.bossesKilled = playerData.bossesKilled;
            
            if (bossesKilled.Count > 0)
            {
                foreach (string boss in bossesKilled)
                {
                    GameObject bossToHide = GameObject.Find(boss);

                    if (bossToHide != null)
                    {
                        bossToHide.SetActive(false);
                    }
                    else
                    {
                        Debug.Log($"Object with name {bossToHide} not found in the scene.");
                    }
                }

                if (bossesKilled.Count == this.totalGameBosses)
                {
                    WinGame();
                }
            }
            this.extraDamage = playerData.extraDamage;
            Debug.Log("base attack: " + playerData.baseDamage + "extra attack: " + playerData.extraDamage);
            this.currentHealth = playerData.currentHealth;

            this.currentHunger = playerData.currentHunger;

            this.spawnPosition = new Vector3(
                playerData.spawnPositionX,
                playerData.spawnPositionY,
                playerData.spawnPositionZ
            );

            playerBody.transform.position = this.spawnPosition;
            GameManager.Instance.loadGameRequest = false;
        }
        else
        {
            ResetSpawnPosition();
            playerBody.transform.position = spawnPosition;

            totalGameBosses = 2;
            extraDamage = 0f;
            healthDecreaseTimer = healthDecreaseInterval;
        }
        dataPending = false;

        if (GameManager.Instance != null && GameManager.Instance.gameDifficulty == 0)
        {
            baseDamage = 35f;
            loseHealthFromHunger = 2f;
            maxHealth = 150f;
        }
        else if (GameManager.Instance != null && GameManager.Instance.gameDifficulty == 1)
        {
            baseDamage = 30f;
            loseHealthFromHunger = 4f;
            maxHealth = 100f;
        }
        else if (GameManager.Instance != null && GameManager.Instance.gameDifficulty == 2)
        {
            baseDamage = 20f;
            loseHealthFromHunger = 6f;
            maxHealth = 80f;
        }

        maxHunger = 100f;
        currentHealth = maxHealth;
        currentHunger = maxHunger;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }


    // Update is called once per frame
    void Update()
    {
        // each frame of distance travelled, it will increase by vector3 distance
        distanceTravelled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;


        if (currentHunger > 0 && distanceTravelled >= 14 && Time.timeSinceLevelLoad > 10.0f)
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
        if (currentHealth <= 0 && !dataPending)
        {
            PauseMenu pauseMenu = menuController.GetComponent<PauseMenu>();
            
            Time.timeScale = 1;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            pauseMenu.LoadGameOverMenu();

        }

        // when the hunger reaches 0, the health will slowly deteriorate
        if (currentHunger == 0)
        {
            healthDecreaseTimer -= Time.deltaTime;
            if (healthDecreaseTimer <= 0)
            {
                currentHealth -= loseHealthFromHunger;
                healthDecreaseTimer = healthDecreaseInterval;
            }
        }
    }

    public void UpdatePlayer(int difficulty)
    {
        if (difficulty == 0)
        {
            baseDamage = 35f;
            loseHealthFromHunger = 2f;
            maxHealth = 150f;
        }
        else if (difficulty == 1)
        {
            baseDamage = 30f;
            loseHealthFromHunger = 4f;
            maxHealth = 100f;
        }
        else if (difficulty == 2)
        {
            baseDamage = 20f;
            loseHealthFromHunger = 6f;
            maxHealth = 80f;
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
