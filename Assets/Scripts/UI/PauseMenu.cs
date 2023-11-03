using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject pauseMenuScreen;
    public GameObject instructionsPage1;
    public GameObject instructionsPage2;

    private bool gameIsPaused = false;


    public bool GameIsPaused() {  return gameIsPaused; }

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

    public void ResumeGame()
    {
        
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameIsPaused = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameIsPaused = true;
    }

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

    public void QuitGame()
    {
        Application.Quit();
    }
}
