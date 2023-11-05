// https://www.youtube.com/watch?v=KR4qUFGuKyQ

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Create Crafting Recepie")]

public class CraftingRecepie : ScriptableObject
{
    public List<ItemAmount> materials;
    public Item CraftedItem;
}
[Serializable]
public struct ItemAmount
{
    public float amount;
    public Item item;
}

