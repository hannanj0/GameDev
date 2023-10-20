using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // might not need this
using TMPro;

public class HealthBar : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI healthCount; // Use TextMeshProUGUI

    public GameObject playerState;

    private float currentHealth, maxHealth;

    void Start()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        currentHealth = playerState.GetComponent<PlayerState>().currentHealth;
        maxHealth = playerState.GetComponent<PlayerState>().maxHealth;

        float fillValue = currentHealth / maxHealth;
        slider.value = fillValue;

        healthCount.text = currentHealth + "/" + maxHealth;
    }
}
