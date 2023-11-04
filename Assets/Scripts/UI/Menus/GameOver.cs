using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The GameOver script controls the game over screen after the user fails to complete the game.
/// </summary>
public class GameOver : MonoBehaviour
{
    /// <summary>
    /// Resets the game, allowing the user to retry.
    /// The flow of time is resumed.
    /// </summary>
    public void RetryGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Load the main menu.
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Quit the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
