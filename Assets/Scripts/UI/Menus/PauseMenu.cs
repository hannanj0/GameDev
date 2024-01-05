using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

/// <summary>
/// The PauseMenu script controls the pause menu through buttons to resume, quit and show game instructions.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public CloudSave cloudSave;
    public PlayerSettings playerSettings;
    public AudioSource buttonClick;

    public Animator fadeScene;

    public Slider sensitivitySlider;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    public AudioMixer audioMixer;
    public TMP_Dropdown graphicsDropdown;
    public TMP_Dropdown difficultyDropdown;

    public Toggle fullScreenToggle;
    public Toggle muteToggle;

    public GameObject pauseMenu; // PauseMenu object to hide and set visible.
    public GameObject pauseMenuScreen; // Pause menu screen object to hide and set visible.
    public GameObject instructionsPage1; // Instructions page 1 object to hide and set visible.
    public GameObject instructionsPage2; // Instructions page 2 object to hide and set visible.
    public GameObject settingsPage; // Settings page object to hide and set visible.
    public GameObject mainMenuDialog;
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

    private bool gameIsPaused = false;
    public PlayerState playerState;

    /// <summary>
    /// Check whether the game is paused or not.
    /// </summary>
    /// <returns> Boolean indicating paused or unpaused state. </returns>
    public bool GameIsPaused() { return gameIsPaused; }

    void Start()
    {
        unselected = new Color(0.7098f, 0.8509f, 0.6275f);
        selected = new Color(0.388f, 0.565f, 0.278f);
    }

    public void ButtonClick()
    {
        buttonClick.Play();
    }

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

    /// <summary>
    /// Toggle between pausing the game and resuming the game.
    /// </summary>
    public void OnPause()
    {
        if (gameIsPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    /// <summary>
    /// Resume the game by allowing time to continue, showing the cursor and setting the boolean.
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        HideTabs();
        LoadPauseMenu();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameIsPaused = false;
    }

    /// Pauses the game by pausing time, hiding the cursor, and setting the boolean.
    private void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameIsPaused = true;
    }

    /// <summary>
    /// Load the pause menu and check to hide instruction pages.
    /// </summary>
    public void LoadPauseMenu()
    {
        pauseMenuScreen.SetActive(true);

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
    /// Load the first instructions page and check to hide the pause menu and second instructions page.
    /// </summary>
    public void LoadInstructionsPage1()
    {
        instructionsPage1.SetActive(true);

        if (pauseMenuScreen.activeSelf)
        {
            pauseMenuScreen.SetActive(false);
        }

        if (instructionsPage2.activeSelf)
        {
            instructionsPage2.SetActive(false);
        }
    }

    /// <summary>
    /// Load the second instructions page and check to hide the pause menu and first instructions page.
    /// </summary>
    public void LoadInstructionsPage2()
    {
        instructionsPage2.SetActive(true);

        if (pauseMenuScreen.activeSelf)
        {
            pauseMenuScreen.SetActive(false);
        }

        if (instructionsPage1.activeSelf)
        {
            instructionsPage1.SetActive(false);
        }
    }

    public void LoadSettings()
    {
        pauseMenuScreen.SetActive(false);
        settingsPage.SetActive(true);
        HideTabs();
        LoadGeneralSettings();
    }

    public void SaveSettings()
    {
        cloudSave.SavePlayerSettings();
    }

    public void SaveGame()
    {
        cloudSave.SaveGame();
    }

    public void LoadMainMenu()
    {
        mainMenuDialog.SetActive(true);
    }

    public void LoadMainMenu_Yes()
    {
        Time.timeScale = 1;
        GameManager.Instance.inGame = false;
        StartCoroutine(GoToMenu());
    }

    IEnumerator GoToMenu()
    {
        fadeScene.SetTrigger("FadeOut");

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadSceneAsync(0);
    }

    public void LoadGameOverMenu() 
    {
        GameManager.Instance.inGame = false;
        StartCoroutine(GameOverMenu()); 
    }

    IEnumerator GameOverMenu()
    {
        Time.timeScale = 1;
        fadeScene.SetTrigger("FadeOut");

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(2);
    }

    public void LoadMainMenu_No()
    {
        mainMenuDialog.SetActive(false);
    }

    public void QuitGame()
    {
        quitGameDialog.SetActive(true);
    }

    public void QuitGame_Yes()
    {
        Application.Quit();
    }

    public void QuitGame_No()
    {
        quitGameDialog.SetActive(false);
    }

    public void LoadGeneralSettings()
    {
        HideTabs();
        ResetButtons();
        SelectButton(generalButton);
        generalSettings.SetActive(true);
    }

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

    public void LoadGraphicsSettings()
    {
        HideTabs();
        ResetButtons();
        SelectButton(graphicsButton);
        graphicsSettings.SetActive(true);
    }

    public void LoadAudioSettings()
    {
        HideTabs();
        ResetButtons();
        SelectButton(audioButton);
        audioSettings.SetActive(true);
    }

    public void LoadAccessibilitySettings()
    {
        HideTabs();
        ResetButtons();
        SelectButton(accessibilityButton);
        accessibilitySettings.SetActive(true);
    }

    public void HideTabs()
    {
        generalSettings.SetActive(false);
        controlsSettings.SetActive(false);
        graphicsSettings.SetActive(false);
        audioSettings.SetActive(false);
        accessibilitySettings.SetActive(false);
        ResetButtons();
    }

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

    public void ToggleFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
        fullScreenToggle.isOn = isFullScreen;
        playerSettings.fullScreen = isFullScreen;
    }

    public void SetGameDifficulty(int index)
    {
        GameManager.Instance.gameDifficulty = index;
        difficultyDropdown.value = index;
        playerSettings.gameDifficulty = index;
        playerState.UpdatePlayer(index);
    }

    public void SetGraphicsQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        graphicsDropdown.value = index;
        playerSettings.graphicsSetting = index;
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
        masterVolumeSlider.value = volume;
        playerSettings.masterVolume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
        musicVolumeSlider.value = volume;
        playerSettings.musicVolume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);
        sfxVolumeSlider.value = volume;
        playerSettings.sfxVolume = volume;
    }

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

    public void ChangeSensitivity(System.Single newSensitivity)
    {
        sensitivitySlider.value = newSensitivity;
        playerSettings.sensitivity = newSensitivity;
    }

    public void LoadBindingsSettings()
    {
        controlsSettingsPage1.SetActive(false);
        settingsBackButton.SetActive(false);
        bindingsBackButton.SetActive(true);
        bindingsSettings.SetActive(true);
    }
}


