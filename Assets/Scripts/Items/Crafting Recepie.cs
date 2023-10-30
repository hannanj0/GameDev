using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Create Crafting Recepie")]
public class CraftingRecepie : ScriptableObject
{
    public List<ItemAmount> materials;
}
[Serializable]
public struct ItemAmount
{
    public float amount;
    public Item item;
}