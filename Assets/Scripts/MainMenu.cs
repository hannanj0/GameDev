using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // this will load the scene from the build settings, 1 is the Main scene
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
