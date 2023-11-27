using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The GameOver script controls the game over screen after the user fails to complete the game.
/// </summary>
public class GameOver : MonoBehaviour
{
    public GameObject mainMenuDialog;
    public GameObject quitGameDialog;

    /// <summary>
    /// Resets the game, allowing the user to retry.
    /// The flow of time is resumed.
    /// </summary>
    public void RetryGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
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
}
