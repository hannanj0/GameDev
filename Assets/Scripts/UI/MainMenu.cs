using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject mainMenuScreen;
    public GameObject instructionsScreen;

    void Start()
    {
    }

    // this will load the scene from the build settings, 1 is the Main scene
    public void PlayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(1);
    }

    public void LoadInstructions()
    {
        mainMenuScreen.SetActive(false);
        instructionsScreen.SetActive(true);
    }

    public void BackToMainMenu()
    {
        mainMenuScreen.SetActive(true);
        instructionsScreen.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
