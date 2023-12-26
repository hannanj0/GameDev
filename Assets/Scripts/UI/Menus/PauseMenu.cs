using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

/// <summary>
/// The PauseMenu script controls the pause menu through buttons to resume, quit and show game instructions.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public CloudSave cloudSave;
    public PlayerSettings playerSettings;

    public Animator fadeScene;

    public Slider sensitivitySlider;
    public Slider masterVolumeSlider;
    public AudioMixer audioMixer;
    public TMP_Dropdown dropdown;
    public Toggle toggle;

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

    public void LoadCloudSettings()
    {
        if (GameManager.Instance != null && GameManager.Instance.loadSettingsRequest)
        {
            playerSettings = GameManager.Instance.playerSettings;
            ToggleFullScreen(playerSettings.fullScreen);
            ChangeSensitivity(playerSettings.sensitivity);
            SetGraphicsQuality(playerSettings.graphicsSetting);
            Debug.Log("graphics: " + playerSettings.graphicsSetting);
            SetMasterVolume(playerSettings.masterVolume);
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
    private void ResumeGame()
    {
        PlayerPrefs.Save();
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
        GameManager.Instance.inGame = false;
        StartCoroutine(GoToMenu());
    }

    IEnumerator GoToMenu()
    {
        Time.timeScale = 1;
        fadeScene.SetTrigger("FadeOut");

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(0);
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
        toggle.isOn = isFullScreen;
        playerSettings.fullScreen = isFullScreen;
    }

    public void SetGraphicsQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
        dropdown.value = index;
        playerSettings.graphicsSetting = index;
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
        masterVolumeSlider.value = volume;
        playerSettings.masterVolume = volume;
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


