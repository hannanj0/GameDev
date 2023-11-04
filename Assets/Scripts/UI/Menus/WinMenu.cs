using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The PauseMenu script controls the win menu after the user beats the game.
/// The user can play again, go back to the main menu or quit the game.
/// </summary>
public class WinMenu : MonoBehaviour
{
    /// <summary>
    /// Resume the game by allowing time to flow again. Loads the Main scene in order to replay the game.
    /// </summary>
    public void PlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Load the main menu screen.
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Quite the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
