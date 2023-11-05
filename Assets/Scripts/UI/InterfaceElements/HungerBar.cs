using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// The HungerBar script is to showcase the player's hunger throughout the game, which changes value depending on the type of interactions taken
/// </summary>
public class HungerBar : MonoBehaviour
{
    private Slider slider; // This is a reference to the UI slider
    public TextMeshProUGUI hungerCount; // This is a reference to the TMP text which displays the hunger count

    public GameObject playerState; // This is a reference to the player's state script
    public GameObject playerMovement; // This is a reference to the player's movement script

    private float currentHunger, maxHunger; // Variables to store the current and max hunger values

    /// <summary>
    /// This initialises the slider by getting it from the GameObject
    /// </summary>
    void Start()
    {
        slider = GetComponent<Slider>();
    }
    
    /// <summary>
    /// This will determine what the fill value of the Hunger Bar slider should be, as well as ensuring the Player can sprint only if the current hunger is greater than 0
    /// </summary>
    void Update()
    {
        currentHunger = playerState.GetComponent<PlayerState>().currentHunger;
        maxHunger = playerState.GetComponent<PlayerState>().maxHunger;

        float fillValue = currentHunger / maxHunger;
        slider.value = fillValue;

        hungerCount.text = currentHunger + "/" + maxHunger;

        
        bool canSprint = currentHunger > 0;
        playerMovement.GetComponent<PlayerMovement>().UpdateSprinting(canSprint);
    }
}
