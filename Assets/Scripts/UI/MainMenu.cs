using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public GameObject mainMenuScreen;
    public GameObject instructionsPage1;
    public GameObject instructionsPage2;

    void Start()
    {
    }

    // this will load the scene from the build settings, 1 is the Main scene
    public void PlayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(1);
    }

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

    public void QuitGame()
    {
        Application.Quit();
    }
}
