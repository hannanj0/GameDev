using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine.InputSystem;


public class CloudSave : MonoBehaviour
{
    PlayerData dataToSave;
    PlayerData loadedData;

    // Start is called before the first frame update
    void Start()
    {
        SetupSignIn();
    }

    async void SetupSignIn()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn) { 
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }


    public void PrepareDataFile()
    {
        dataToSave = new PlayerData();
        dataToSave.bossesKilled.Add("LavaBoss");
    }
    
    public async void SaveDataFile()
    {
        try
        {
            // Serialize the PlayerData object to JSON
            string JSONData = JsonConvert.SerializeObject(dataToSave);
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(JSONData);

            // Save the JSON data to Unity Cloud Saves
            await CloudSaveService.Instance.Files.Player.SaveAsync("playerData", byteArray);

            Debug.Log("Player data saved to Unity Cloud");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to save player data: {e.Message}");
        }
    }

    public async void LoadDataFile()
    {
        try
        {
            // Retrieve the JSON data from Unity Cloud Saves
            byte[] byteArray = await CloudSaveService.Instance.Files.Player.LoadBytesAsync("playerData");
            string JSONData = System.Text.Encoding.UTF8.GetString(byteArray);
            loadedData = JsonUtility.FromJson<PlayerData>(JSONData);
            // Deserialize the JSON data back into the PlayerData object


            Debug.Log("Player data loaded from Unity Cloud");
            Debug.Log(loadedData.bossesKilled);
            Debug.Log(loadedData.currentHealth);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to load player data: {e.Message}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check for "s" key press
        if (Keyboard.current.sKey.wasPressedThisFrame)
        {
            PrepareDataFile();
            SaveDataFile();
            //SaveData();
            // Do something when "s" key is pressed
        }

        // Check for "d" key press
        if (Keyboard.current.dKey.wasPressedThisFrame)
        {
            LoadDataFile();
            //LoadData();
            // Do something when "d" key is pressed
        }
    }
}
