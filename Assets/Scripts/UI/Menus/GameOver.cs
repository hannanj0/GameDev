using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The GameOver script controls the game over screen after the user fails to complete the game.
/// </summary>
public class GameOver : MonoBehaviour
{
    public Animator fadeScene;
    public AudioSource buttonClick;
    public GameObject mainMenuDialog;
    public GameObject quitGameDialog;

    public void ButtonClick()
    {
        buttonClick.Play();
    }

    /// <summary>
    /// Resets the game, allowing the user to retry.
    /// The flow of time is resumed.
    /// </summary>
    public void RetryGame()
    {
        GameManager.Instance.loadGameRequest = true;
        GameManager.Instance.inGame = true;
        Time.timeScale = 1;
        StartCoroutine(Retry());
    }
    /// <summary>
    /// Starts the game again
    /// </summary>
    IEnumerator Retry()
    {
        fadeScene.SetTrigger("FadeOut");

        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadSceneAsync(1);
    }
    /// <summary>
    /// Takes the player to the main menu
    /// </summary>
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
        GameManager.Instance.inGame = false;
        StartCoroutine(MainMenu());
    }

    public void LoadMainMenu_No()
    {
        mainMenuDialog.SetActive(false);
    }
    /// <summary>
    /// Quits the game
    /// </summary>
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
