using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The GameOver script controls the game over screen after the user fails to complete the game.
/// </summary>
public class GameOver : MonoBehaviour
{
    public Animator fadeScene;

    public GameObject mainMenuDialog;
    public GameObject quitGameDialog;

    /// <summary>
    /// Resets the game, allowing the user to retry.
    /// The flow of time is resumed.
    /// </summary>
    public void RetryGame()
    {
        GameManager.Instance.loadGameRequest = true;
        Time.timeScale = 1;
        StartCoroutine(Retry());
    }

    IEnumerator Retry()
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

    public void LoadMainMenu()
    {
        mainMenuDialog.SetActive(true);
    }

    public void LoadMainMenu_Yes()
    {
        Time.timeScale = 1;
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
