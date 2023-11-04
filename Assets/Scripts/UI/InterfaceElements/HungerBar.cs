using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HungerBar : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI hungerCount;

    public GameObject playerState;
    public GameObject playerMovement; // Reference to the PlayerMovement script

    private float currentHunger, maxHunger;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        currentHunger = playerState.GetComponent<PlayerState>().currentHunger;
        maxHunger = playerState.GetComponent<PlayerState>().maxHunger;

        float fillValue = currentHunger / maxHunger;
        slider.value = fillValue;

        hungerCount.text = currentHunger + "/" + maxHunger;

        // Check if the player's hunger has reached 0 and update sprinting in PlayerMovement
        bool canSprint = currentHunger > 0; // Allow sprinting if hunger is above 0
        playerMovement.GetComponent<PlayerMovement>().UpdateSprinting(canSprint);
    }
}
