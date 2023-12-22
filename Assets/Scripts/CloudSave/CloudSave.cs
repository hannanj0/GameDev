using UnityEngine;
using Newtonsoft.Json;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine.InputSystem;
using System.Threading.Tasks;

public class CloudSave : MonoBehaviour
{
    PlayerData dataToSave;
    PlayerData loadedData;

    public bool dataFound = false;
    private TaskCompletionSource<bool> loadGameCompletionSource;

    public PlayerState playerState;


    // Start is called before the first frame update
    void Start()
    {
        SetupSignIn();
    }

    void Update()
    {
        // Check for "k" key press
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            PrepareDataFile();
            SaveDataFile();
        }

        // Check for "l" key press
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadGame();
        }
    }

    async void SetupSignIn()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn) { 
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public void SaveGame()
    {
        PrepareDataFile();
        SaveDataFile();
    }

    private void PrepareDataFile()
    {
        //player = GameObject.Find("PlayerAsset");
        //playerState = player.GetComponent<PlayerState>();
        Debug.Log("data"+playerState.SpawnX());

        dataToSave = new PlayerData {
            bossesKilled = playerState.BossesKilled(),

            currentHealth = playerState.CurrentHealth(),
            maxHealth = playerState.MaxHealth(),
            currentHunger = playerState.CurrentHunger(),
            maxHunger = playerState.MaxHunger(),
            attackDamage = playerState.AttackDamage(),

            spawnPositionX = playerState.SpawnX(),
            spawnPositionY = playerState.SpawnY(),
            spawnPositionZ = playerState.SpawnZ(),
        };
        Debug.Log("cloud save " + dataToSave.attackDamage);
    }
    
    private async void SaveDataFile()
    {
        try
        {
            // Serialize the PlayerData object to JSON
            string JSONData = JsonConvert.SerializeObject(dataToSave);
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(JSONData);

            // Save the JSON data to Unity Cloud Saves
            await CloudSaveService.Instance.Files.Player.SaveAsync("playerData", byteArray);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save player data: {e.Message}");
        }
    }

    public Task<bool> LoadGame()
    {
        loadGameCompletionSource = new TaskCompletionSource<bool>();
        LoadGameAsync();
        return loadGameCompletionSource.Task;
    }

    private async void LoadGameAsync()
    {
        try
        {
            await LoadDataFile(); // Wait for the asynchronous operation to complete

            // Set the result of the task completion source
            loadGameCompletionSource.SetResult(dataFound);

            if (dataFound)
            {
                dataFound = false;
            }
        }
        catch (System.Exception e)
        {
            // Set the exception if an error occurs
            loadGameCompletionSource.SetException(e);
            Debug.LogError($"Failed to load player data: {e.Message}");
        }
    }

    private async Task LoadDataFile()
    {
        try
        {
            byte[] byteArray = await CloudSaveService.Instance.Files.Player.LoadBytesAsync("playerData");
            
            if (byteArray != null && byteArray.Length > 0)
            {
                // Retrieve the JSON data from Unity Cloud Saves
                string JSONData = System.Text.Encoding.UTF8.GetString(byteArray);
                loadedData = JsonUtility.FromJson<PlayerData>(JSONData);
                // Deserialize the JSON data back into the PlayerData object
                dataFound = true;
                GameManager.Instance.playerData = loadedData;

            }
            else
            {
                // No save found in the cloud
                Debug.Log("No cloud save found.");
            }
           
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load player data: {e.Message}");
        }
    }
}
