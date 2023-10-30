using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    //public GameObject pauseMenu;

    //private bool gameIsPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Escape))
    //    {
    //        if (gameIsPaused)
    //        {
    //            ResumeGame();
    //        }

    //        else
    //        {
    //            PauseGame();
    //        }
    //    }
    //}

    public void ResumeGame()
    {
        
        Time.timeScale = 1;
        //pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //gameIsPaused = false;
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        //pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //gameIsPaused = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
