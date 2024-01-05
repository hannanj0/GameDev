using UnityEngine;
using Newtonsoft.Json;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine.InputSystem;
using System.Threading.Tasks;
using System.IO;

public class CloudSave : MonoBehaviour
{
    PlayerData dataToSave;
    PlayerData loadedData;

    PlayerSettings settingsToSave;
    PlayerSettings settingsLoaded;

    public bool dataFound = false;
    public bool settingsFound = false;
    private TaskCompletionSource<bool> loadGameCompletionSource;
    private TaskCompletionSource<bool> loadSettingsCompletionSource;

    public PlayerState playerState;
    public MainMenu mainMenu;
    public PauseMenu pauseMenu;

    // Start is called before the first frame update
    async void Start()
    {
        await SetupSignIn();
        await LoadSettings();

    }

    void Update()
    {
        // Check for "k" key press
        if (Keyboard.current.kKey.wasPressedThisFrame)
        {
            PreparePlayerSettings();
            SaveSettingsFile();
            Debug.Log("saved");
        }

        // Check for "l" key press
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            LoadSettings();
            Debug.Log("loaded");
        }
    }

    async Task SetupSignIn()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn) { 
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public void SaveGame()
    {
        PreparePlayerData();
        SaveDataFile();
    }

    public void SavePlayerSettings()
    {
        PreparePlayerSettings();
        SaveSettingsFile();
    }

    private void PreparePlayerSettings()
    {
        if (GameManager.Instance.inGame) 
        {
            settingsToSave = pauseMenu.playerSettings;
        }
        else
        {
            settingsToSave = mainMenu.playerSettings;
        }

    }

    private void PreparePlayerData()
    {
        Debug.Log(playerState.SpawnX());
        Debug.Log(playerState.SpawnY());
        Debug.Log(playerState.SpawnZ());
        dataToSave = new PlayerData {
            bossesKilled = playerState.BossesKilled(),

            currentHealth = playerState.CurrentHealth(),
            maxHealth = playerState.MaxHealth(),
            currentHunger = playerState.CurrentHunger(),
            maxHunger = playerState.MaxHunger(),
            baseDamage = playerState.BaseDamage(),
            extraDamage = playerState.ExtraDamage(),

            spawnPositionX = playerState.SpawnX(),
            spawnPositionY = playerState.SpawnY(),
            spawnPositionZ = playerState.SpawnZ(),
        };
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

    private async void SaveSettingsFile()
    {
        try
        {
            // Serialize the PlayerData object to JSON
            Debug.Log("before save: "+settingsToSave.graphicsSetting);
            string JSONData = JsonConvert.SerializeObject(settingsToSave);
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(JSONData);

            // Save the JSON data to Unity Cloud Saves
            await CloudSaveService.Instance.Files.Player.SaveAsync("playerSettings", byteArray);
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

    public Task<bool> LoadSettings()
    {
        loadSettingsCompletionSource = new TaskCompletionSource<bool>();
        LoadSettingsAsync();
        return loadSettingsCompletionSource.Task;
    }

    private async void LoadSettingsAsync()
    {
        try
        {
            await LoadSettingsFile(); // Wait for the asynchronous operation to complete

            // Set the result of the task completion source
            loadSettingsCompletionSource.SetResult(settingsFound);

            if (settingsFound)
            {
                settingsFound = false;
            }
        }
        catch (System.Exception e)
        {
            // Set the exception if an error occurs
            loadSettingsCompletionSource.SetException(e);
            Debug.LogError($"Failed to load player data: {e.Message}");
        }
    }

    private async Task LoadSettingsFile()
    {
        try
        {
            byte[] byteArray = await CloudSaveService.Instance.Files.Player.LoadBytesAsync("playerSettings");

            if (byteArray != null && byteArray.Length > 0)
            {
                // Retrieve the JSON data from Unity Cloud Saves
                string JSONData = System.Text.Encoding.UTF8.GetString(byteArray);
                settingsLoaded = JsonUtility.FromJson<PlayerSettings>(JSONData);
                // Deserialize the JSON data back into the PlayerData object
                settingsFound = true;
                GameManager.Instance.playerSettings = settingsLoaded;
                GameManager.Instance.loadSettingsRequest = true;
                Debug.Log(GameManager.Instance.inGame);
                if (GameManager.Instance.inGame)
                {
                    pauseMenu.LoadCloudSettings();
                }
                else
                {
                    mainMenu.LoadCloudSettings();
                }

            }
            else
            {
                // No save found in the cloud
                Debug.Log("No cloud save found.");
            }

        }
        catch (CloudSaveException)
        {
            Debug.Log("no saved settings in cloud yet");
        }

        catch
        {
            Debug.Log("no data loaded");
        }
    }
}
