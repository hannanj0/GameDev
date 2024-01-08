using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ItemDescription : MonoBehaviour
{
    private bool offCooldown;
    private float itemDescriptionTimer = 10.0f;
    private Image descriptionImage;
    private Color backgroundColour;
    private TextMeshProUGUI descriptionText;
    private float timer;
    void Start()
    {
        offCooldown = true;
        descriptionImage = GetComponent<Image>();
        backgroundColour = descriptionImage.color;
        Transform textMesh = transform.Find("DescriptionDisplay");
        descriptionText = textMesh.GetComponent<TextMeshProUGUI>();
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
                backgroundColour.a = 0f;
                descriptionImage.color = backgroundColour;
                descriptionText.text = null;

            }
        }
    }
    public void DisplayDescription(GameItem item)
    {
        offCooldown = false;
        backgroundColour.a = 100 / 255f;
        descriptionImage.color = backgroundColour;
        timer = itemDescriptionTimer;
        Debug.Log(descriptionText);
        descriptionText.text = item.description;

    }

}
