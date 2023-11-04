using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The MainMenu script controls the main menu, allowing you to play the game, view the game instructions or quit the game.
/// </summary>
public class MainMenu : MonoBehaviour
{
    public GameObject mainMenuScreen;
    public GameObject instructionsPage1;
    public GameObject instructionsPage2;

    /// <summary>
    /// Load the game and set time to flow at the normal rate.
    /// </summary>
    public void PlayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(1);
    }

    /// <summary>
    /// Load the main menu screen and hide the instructions pages.
    /// </summary>
    public void LoadMainMenu()
    {
        mainMenuScreen.SetActive(true);

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
    /// Load the first instructions page and check to hide the main menu and second instructions page.
    /// </summary>
    public void LoadInstructionsPage1()
    {
        instructionsPage1.SetActive(true);

        if (mainMenuScreen.activeSelf)
        {
            mainMenuScreen.SetActive(false);
        }

        if (instructionsPage2.activeSelf)
        {
            instructionsPage2.SetActive(false);
        }
    }

    /// <summary>
    /// Load the second instructions page and check to hide the main menu and first instructions page.
    /// </summary>
    public void LoadInstructionsPage2()
    {
        instructionsPage2.SetActive(true);

        if (mainMenuScreen.activeSelf)
        {
            mainMenuScreen.SetActive(false);
        }

        if (instructionsPage1.activeSelf)
        {
            instructionsPage1.SetActive(false);
        }
    }

    /// <summary>
    /// Quit the game.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}

