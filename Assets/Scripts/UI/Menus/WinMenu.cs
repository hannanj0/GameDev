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
    public GameObject playAgainDialog;
    public GameObject mainMenuDialog;
    public GameObject quitGameDialog;

    /// <summary>
    /// Resume the game by allowing time to flow again. Loads the Main scene in order to replay the game.
    /// </summary>
    public void PlayAgain()
    {
        playAgainDialog.SetActive(true);
    }

    public void PlayAgain_Yes()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    public void PlayAgain_No()
    {
        playAgainDialog.SetActive(false);
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
