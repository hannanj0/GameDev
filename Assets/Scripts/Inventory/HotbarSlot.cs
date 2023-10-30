using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class HotbarSlot : MonoBehaviour
{
    public Item assignedItem;
    public Image hotbarImage;
    public Color imageColor;
    public Boolean isEmpty = true;
    public TextMeshProUGUI textMesh;


    void Start()
    {
        hotbarImage = GetComponent<Image>();
        imageColor = hotbarImage.color;
        Transform itemName = transform.Find("ItemName");
        textMesh = itemName.GetComponent<TextMeshProUGUI>();
        Debug.Log(textMesh);
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
        //hotbarImage.sprite = item.hotbar_image;
        textMesh.text = item.Name;
        Debug.Log(textMesh.text);
        isEmpty = false;
    }
    public void RemoveItem()
    {
        assignedItem = null;
        //hotbarImage.sprite = null;
        textMesh.text = null;
        isEmpty = true;
    }


}
