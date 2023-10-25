using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void RetryGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

    // this will load the scene from the build settings, 1 is the Main scene
    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
