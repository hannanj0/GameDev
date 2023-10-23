using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // might not need this
using TMPro;

public class HungerBar : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI hungerCount; // Use TextMeshProUGUI

    public GameObject playerState;

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
    }
}
