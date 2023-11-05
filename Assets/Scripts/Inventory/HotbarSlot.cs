using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// A script for the hotbar slot in the game
/// Used for selecting items in your hotbar to use them
/// Displays the items the player currently has
/// </summary>
public class HotbarSlot : MonoBehaviour
{
    public Item assignedItem;
    public Image hotbarImage;
    public Color imageColor;
    public Boolean isEmpty = true;
    public TextMeshProUGUI textMesh;

    /// <summary>
    /// Initialises the fields
    /// </summary>
    void Start()
    {
        hotbarImage = GetComponent<Image>();
        imageColor = hotbarImage.color;
        Transform itemName = transform.Find("ItemName");
        textMesh = itemName.GetComponent<TextMeshProUGUI>();
        Debug.Log(textMesh);
    }
    /// <summary>
    /// Called when the hotbar is selected
    /// </summary>
    public void Select()
    {
        imageColor.a = 1f;
        hotbarImage.color = imageColor;
    }
    /// <summary>
    /// Called when another hotbar is selected
    /// </summary>
    public void Deselect()
    {
        imageColor.a = 100/255f;
        hotbarImage.color = imageColor;
    }
    /// <summary>
    /// Assigns the item to the hotbar
    /// </summary>
    public void AssignItem(Item item)
    {
        assignedItem = item;
        textMesh.text = item.Name;
        isEmpty = false;
    }
    /// <summary>
    /// Removes the item from the hotbar
    /// </summary>
    public void RemoveItem()
    {
        assignedItem = null;
        textMesh.text = null;
        isEmpty = true;
    }


}
