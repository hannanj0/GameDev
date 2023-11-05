using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to craft items
/// Creates a scriptable object that has an array of items needed to craft the desired item
/// </summary>
[CreateAssetMenu(fileName = "Item", menuName = "Create Crafting Recepie")]

public class CraftingRecepie : ScriptableObject
{
    public List<ItemAmount> materials;
    public Item CraftedItem;
}
/// <summary>
/// Fields of the array
/// </summary>
[Serializable]
public struct ItemAmount
{
    public float amount;
    public Item item;
}

