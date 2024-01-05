using UnityEngine;

public class IntroCutsceneManager : MonoBehaviour
{
    public GameObject[] uiElements;

    void Start()
    {
        // Start the intro cutscene
        StartIntroCutscene();
    }

    void StartIntroCutscene()
    {
        // Disable multiple UI elements during the intro cutscene
        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(false);
        }

        // Schedule the end of the cutscene after 14 seconds
        Invoke("EndIntroCutscene", 14f);
    }

    void EndIntroCutscene()
    {
        // Enable multiple UI elements after the intro cutscene
        foreach (GameObject uiElement in uiElements)
        {
            uiElement.SetActive(true);
        }
    }
}
