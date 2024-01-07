using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/// <summary>
/// The MainMenu script controls the main menu, allowing you to play the game, view the game instructions or quit the game.
/// </summary>
public class MainMenu : MonoBehaviour
{
    public AudioSource buttonClick;
    public CloudSave cloudSave;
    public PlayerSettings playerSettings;

    public Animator fadeScene;
    public Animator fadeMusic;

    public Slider sensitivitySlider;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    public AudioMixer audioMixer;
    public TMP_Dropdown graphicsDropdown;
    public TMP_Dropdown difficultyDropdown;

    public Toggle fullScreenToggle;
    public Toggle muteToggle;

    public GameObject mainMenuScreen;
    public GameObject instructionsPage1;
    public GameObject instructionsPage2;
    public GameObject settingsPage;

    public GameObject newGameDialog;
    public GameObject loadGameDialog;
    public GameObject noSavesFoundDialog;
    public GameObject quitGameDialog;

    public GameObject generalSettings;
    public GameObject controlsSettings;
    public GameObject graphicsSettings;
    public GameObject audioSettings;
    public GameObject accessibilitySettings;
    public GameObject controlsSettingsPage1;
    public GameObject bindingsSettings;
    public GameObject settingsBackButton;
    public GameObject bindingsBackButton;

    public Button generalButton;
    public Button controlsButton;
    public Button graphicsButton;
    public Button audioButton;
    public Button accessibilityButton;

    private Color unselected;
    private Color selected;

    void Start()
    {
        //colours for unselected and selected buttons
        unselected = new Color(0.7098f, 0.8509f, 0.6275f);
        selected = new Color(0.388f, 0.565f, 0.278f);
    }

    public void ButtonClick()
    {
        buttonClick.Play();
    }

    //load saved game settings from the cloud
    public void LoadCloudSettings()
    {
        if (GameManager.Instance != null && GameManager.Instance.loadSettingsRequest)
        {
            playerSettings = GameManager.Instance.playerSettings;
            ToggleFullScreen(playerSettings.fullScreen);
            SetGameDifficulty(playerSettings.gameDifficulty);
            ChangeSensitivity(playerSettings.sensitivity);
            SetGraphicsQuality(playerSettings.graphicsSetting);
            SetMasterVolume(playerSettings.masterVolume);
            SetMusicVolume(playerSettings.musicVolume);
            SetSFXVolume(playerSettings.sfxVolume);
            SetToggleMute(playerSettings.isMuted);
            GameManager.Instance.loadSettingsRequest = false;
        }
    }

    //new game dialog pop up
    public void NewGame()
    {
        mainMenuScreen.SetActive(false);
        newGameDialog.SetActive(true);
    }

    /// <summary>
    /// Load the game and set time to flow at the normal rate.
    /// </summary>
    public void NewGame_Yes()
    {
        Time.timeScale = 1;
        GameManager.Instance.loadSettingsRequest = true;
        GameManager.Instance.playerSettings = playerSettings;
        GameManager.Instance.inGame = true;
        StartCoroutine(StartGame());
    }


    // fade into main scene game
    IEnumerator StartGame()
    {
        fadeScene.SetTrigger("FadeOut");
        fadeMusic.SetTrigger("FadeOut");

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadSceneAsync(1);
    }

    public void NewGame_No()
    {
        newGameDialog.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    public void LoadGame()
    {
        mainMenuScreen.SetActive(false);
        loadGameDialog.SetActive(true);
    }

    //if user wants to load game, check if save found
    public async void LoadGame_Yes()
    {
        bool loadGameResult = await cloudSave.LoadGame();
        // save file found
        if (loadGameResult)
        {
            GameManager.Instance.loadGameRequest = true;
            GameManager.Instance.playerSettings = playerSettings;
            GameManager.Instance.inGame = true;
            Time.timeScale = 1;
            HideTabs();
            StartCoroutine(StartGame());
        }
        else
        {
            noSavesFoundDialog.SetActive(true);
            loadGameDialog.SetActive(false);
        }
    }

    public void LoadGame_No()
    {
        loadGameDialog.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    //dialog to confirm no saves found
    public void ConfirmNoSavesFound()
    {
        noSavesFoundDialog.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    /// <summary>
    /// Load the main menu screen and hide the instructions pages.
    /// </summary>
    public void LoadMainMenu()
    {
        mainMenuScreen.SetActive(true);

        if (instructionsPage1.activeSelf)
        {
            instructionsPage1.SetActive(false);
        }

        if (instructionsPage2.activeSelf)
        {
            instructionsPage2.SetActive(false);
        }

        if (settingsPage.activeSelf)
        {
            settingsPage.SetActive(false);
            HideTabs();
        }
    }

    /// <summary>
    /// Load the first instructions page and check to hide the main menu and second instructions page.
    /// </summary>
    public void LoadInstructionsPage1()
    {
        instructionsPage1.SetActive(true);

        if (mainMenuScreen.activeSelf)
        {
            mainMenuScreen.SetActive(false);
        }

        if (instructionsPage2.activeSelf)
        {
            instructionsPage2.SetActive(false);
        }
    }

    /// <summary>
    /// Load the second instructions page and check to hide the main menu and first instructions page.
    /// </summary>
    public void LoadInstructionsPage2()
    {
        instructionsPage2.SetActive(true);

        if (mainMenuScreen.activeSelf)
        {
            mainMenuScreen.SetActive(false);
        }

        if (instructionsPage1.activeSelf)
        {
            instructionsPage1.SetActive(false);
        }
    }

    // load all game settings
    public void LoadSettings()
    {
        mainMenuScreen.SetActive(false);
        settingsPage.SetActive(true);
        HideTabs();
        LoadGeneralSettings();
    }

    //save player settings to the cloud
    public void SaveSettings()
    {
        cloudSave.SavePlayerSettings();
    }

    public void QuitGame()
    {
        mainMenuScreen.SetActive(false);
        quitGameDialog.SetActive(true);
    }

    public void QuitGame_Yes()
    {
        Application.Quit();
    }

    public void QuitGame_No()
    {
        quitGameDialog.SetActive(false);
        mainMenuScreen.SetActive(true);
    }

    //load general settings for the game
    public void LoadGeneralSettings()
    {
        HideTabs();
        ResetButtons();
        SelectButton(generalButton);
        generalSettings.SetActive(true);
    }

    //load control settings for the game
    public void LoadControlsSettings()
    {
        HideTabs();
        ResetButtons();
        SelectButton(controlsButton);
        controlsSettings.SetActive(true);
        controlsSettingsPage1.SetActive(true);
        bindingsSettings.SetActive(false);
        bindingsBackButton.SetActive(false);
        settingsBackButton.SetActive(true);
    }

    // load graphics settings for the game
    public void LoadGraphicsSettings()
    {
        HideTabs();
        ResetButtons();
        SelectButton(graphicsButton);
        graphicsSettings.SetActive(true);
    }

    //load audio settings for the game
    public void LoadAudioSettings()
    {
        HideTabs();
        ResetButtons();
        SelectButton(audioButton);
        audioSettings.SetActive(true);
    }

    //load accessibility settings for the game
    public void LoadAccessibilitySettings()
    {
        HideTabs();
        ResetButtons();
        SelectButton(accessibilityButton);
        accessibilitySettings.SetActive(true);
    }

    //hide all settings tabs and reset button colours
    public void HideTabs()
    {
        generalSettings.SetActive(false);
        controlsSettings.SetActive(false);
        graphicsSettings.SetActive(false);
        audioSettings.SetActive(false);
        accessibilitySettings.SetActive(false);
        ResetButtons();
    }

    //reset button colours
    private void ResetButtons()
    {
        generalButton.image.color = unselected;
        controlsButton.image.color = unselected;
        graphicsButton.image.color = unselected;
        audioButton.image.color = unselected;
        accessibilityButton.image.color = unselected;
    }

    private void SelectButton(Button selectedButton)
    {
        selectedButton.image.color = selected;
    }

    // toggle fullscreen mode
    public void ToggleFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        fullScreenToggle.isOn = isFullScreen;
        playerSettings.fullScreen = isFullScreen;
    }

    //set the game difficulty
    public void SetGameDifficulty(int index)
    {
        GameManager.Instance.gameDifficulty = index;
        difficultyDropdown.value = index;
        playerSettings.gameDifficulty = index;
    }

    //set graphics quality for the game
    public void SetGraphicsQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        graphicsDropdown.value = index;
        playerSettings.graphicsSetting = index;
    }

    //Set the master volume for the game
    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
        masterVolumeSlider.value = volume;
        playerSettings.masterVolume = volume;
    }

    //Set the music volume for the game
    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
        musicVolumeSlider.value = volume;
        playerSettings.musicVolume = volume;
    }

    //Set the SFX volume for the game
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);
        sfxVolumeSlider.value = volume;
        playerSettings.sfxVolume = volume;
    }

    //Toggle mute for the entire game
    public void SetToggleMute(bool isMuted)
    {
        if (isMuted) 
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
        muteToggle.isOn = isMuted;
        playerSettings.isMuted = isMuted;
    }

    // Change sensitivity
    public void ChangeSensitivity(System.Single newSensitivity)
    {
        sensitivitySlider.value = newSensitivity;
        playerSettings.sensitivity = newSensitivity;
    }

    //Load bindings settings
    public void LoadBindingsSettings()
    {
        controlsSettingsPage1.SetActive(false);
        settingsBackButton.SetActive(false);
        bindingsBackButton.SetActive(true);
        bindingsSettings.SetActive(true);
    }
}

