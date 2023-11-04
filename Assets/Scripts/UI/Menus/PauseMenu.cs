using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// The PauseMenu script controls the pause menu through buttons to resume, quit and show game instructions.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu; // PauseMenu object to hide and set visible.
    public GameObject pauseMenuScreen; // Pause menu screen object to hide and set visible.
    public GameObject instructionsPage1; // Instructions page 1 object to hide and set visible.
    public GameObject instructionsPage2; // Instructions page 2 object to hide and set visible.

    private bool gameIsPaused = false;

    /// <summary>
    /// Check whether the game is paused or not.
    /// </summary>
    /// <returns> Boolean indicating paused or unpaused state. </returns>
    public bool GameIsPaused() { return gameIsPaused; }

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
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
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

    /// <summary>
    ///  Quit the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
