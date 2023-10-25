using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName ="Create Item")]
public class Item : ScriptableObject
{
    public string Name;

    public Sprite hotbar_image;
    public Item type;

    public virtual void Use(PlayerState ps){
        Debug.Log("nAH");
    }

}
