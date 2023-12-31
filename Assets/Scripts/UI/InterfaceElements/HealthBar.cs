using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // might not need this
using TMPro;

/// <summary>
/// The HealthBar script is to showcase the player's health throughout the game, which changes value depending on the type of interactions taken
/// </summary>
public class HealthBar : MonoBehaviour
{
    private Slider slider; 
    public TextMeshProUGUI healthCount; 

    public GameObject playerState; 

    private float currentHealth, maxHealth; 

    /// <summary>
    /// This initialises the slider by getting it from the GameObject
    /// </summary>
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    /// <summary>
    /// This will determine what the fill value of the Health Bar slider should be, and displays it
    /// </summary>
    void Update()
    {
        currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
        maxHealth = playerState.GetComponent<PlayerState>().maxHealth;

        float fillValue = currentHealth / maxHealth;
        slider.value = fillValue;

        healthCount.text = currentHealth + "/" + maxHealth;
    }
}
