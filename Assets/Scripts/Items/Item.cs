using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName ="Create Item")]
public class Item : ScriptableObject
{
    public string Name;

    public Sprite hotbar_image;
    public Boolean consumable;

    public virtual void Use(PlayerState ps){
        Debug.Log("nAH");
    }

}
