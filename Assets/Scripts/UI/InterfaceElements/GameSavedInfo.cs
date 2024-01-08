using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// The GameSavedInfo script is used to display a message when the game is saved.
/// </summary>
public class GameSavedInfo : MonoBehaviour
{
    private string message;

    private bool offCooldown;
    private float itemDescriptionTimer = 2.0f;
    private Image descriptionImage;
    private Color backgroundColour;
    private TextMeshProUGUI descriptionText;
    private float timer;
    void Start()
    {
        message = "Game Saved";
        offCooldown = true;
        //backgroundColour = descriptionImage.color;
        descriptionText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!offCooldown)
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            if (timer <= 0)
            {
                //backgroundColour.a = 0f;
                //descriptionImage.color = backgroundColour;
                descriptionText.text = null;

            }
        }
    }
    public void DisplayDescription()
    {
        offCooldown = false;
        //backgroundColour.a = 100 / 255f;
        //descriptionImage.color = backgroundColour;
        timer = itemDescriptionTimer;
        descriptionText.text = message;

    }
}
