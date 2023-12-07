using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// The PauseMenu script controls the pause menu through buttons to resume, quit and show game instructions.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public AudioMixer audioMixer;

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
        unselected = new Color(0.388f, 0.565f, 0.278f);
        selected = new Color(0.145f, 0.294f, 0.118f);
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

    public void LoadMainMenu()
    {
        mainMenuDialog.SetActive(true);
    }

    public void LoadMainMenu_Yes()
    {
        SceneManager.LoadScene(0);
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
    }

    public void SetGraphicsQuality(int index)
    {

        QualitySettings.SetQualityLevel(index);
    }

    public void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
}


