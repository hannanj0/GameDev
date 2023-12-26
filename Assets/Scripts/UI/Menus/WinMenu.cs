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
    public Animator fadeScene;

    public GameObject playAgainDialog;
    public GameObject mainMenuDialog;
    public GameObject quitGameDialog;

    IEnumerator ReplayGame()
    {
        fadeScene.SetTrigger("FadeOut");

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadSceneAsync(1);
    }
    IEnumerator MainMenu()
    {
        fadeScene.SetTrigger("FadeOut");

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadSceneAsync(0);
    }

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
        GameManager.Instance.inGame = true;
        StartCoroutine(ReplayGame());
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
        Time.timeScale = 1;
        GameManager.Instance.inGame = false;
        StartCoroutine(MainMenu());
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
