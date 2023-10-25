using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class HotbarSlot : MonoBehaviour
{
    public Item assignedItem;
    public Image hotbarImage;
    public Color imageColor;
    public Boolean isEmpty = true;

    void Start()
    {
        hotbarImage = GetComponent<Image>();
        imageColor = hotbarImage.color;
    }
    public void Select()
    {
        imageColor.a = 1f;
        hotbarImage.color = imageColor;
    }
    public void Deselect()
    {
        imageColor.a = 100/255f;
        hotbarImage.color = imageColor;
    }
    public void AssignItem(Item item)
    {
        assignedItem = item;
        hotbarImage.sprite = item.hotbar_image;

        isEmpty = false;
    }
    public void RemoveItem()
    {
        assignedItem = null;
        hotbarImage.sprite = null;
        isEmpty = true;
    }


}
