using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Creates a scriptable object called item
/// Gives the item a name and declares if the item is consumable or not
/// </summary>
[CreateAssetMenu(fileName = "Item", menuName ="Create Item")]
public class Item : ScriptableObject
{
    public string Name;
    public Boolean consumable;

    public virtual void Use(PlayerState ps){return;}

}
