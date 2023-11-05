using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftingBar : MonoBehaviour
{
    private CraftingMaterial Material1;
    private TextMeshProUGUI Material1Name;
    private CraftingMaterial Material2;
    private TextMeshProUGUI Material2Name;
    private CraftedItem CraftedItem;
    private TextMeshProUGUI CIName;
    private Inventory inventory;

    /// <summary>
    /// Initialises the fields
    /// Sets the name and the amount in the UI
    /// </summary>
    void Start()
    {
        Transform Mat1 = transform.Find("Material1");
        Material1 = Mat1.GetComponent<CraftingMaterial>();

        Transform Mat2 = transform.Find("Material2");
        Material2 = Mat2.GetComponent<CraftingMaterial>();

        Transform CI = transform.Find("ItemObtained");
        CraftedItem = CI.GetComponent<CraftedItem>();

        Transform Mat1N = transform.Find("Material1/ItemName");
        Material1Name = Mat1N.GetComponent<TextMeshProUGUI>();

        Transform Mat2N = transform.Find("Material2/ItemName");
        Material2Name = Mat2N.GetComponent<TextMeshProUGUI>();

        Transform CIN = transform.Find("ItemObtained/ItemName");
        CIName = CIN.GetComponent<TextMeshProUGUI>();

        Transform PI = transform.parent.parent.parent.Find("Inventory");
        inventory = PI.GetComponent<Inventory>();
        
        Material1.Item = CraftedItem.CR.materials[0].item;
        Material1.count = CraftedItem.CR.materials[0].amount;

        Material2.Item = CraftedItem.CR.materials[1].item;
        Material2.count = CraftedItem.CR.materials[1].amount;

        Material1Name.text = Material1.Item.name;
        Material2Name.text = Material2.Item.name;

        CIName.text = CraftedItem.CR.CraftedItem.name;
    }
    /// <summary>
    /// Crafts the item
    /// Checks if both of the items needed to craft the final item are avalible
    /// Removes the items first so that if the inventory is initially full the item can still be crafted and assigned
    /// Lastly addds the item to the inventory
    /// </summary>
    public void Craft()
    {
        Boolean m1 = false;
        Boolean m2 = false;
        int m1_loc = 0;
        int m2_loc = 0;

        for(int i = 0; i < inventory.hotbarSlots.Length; i++)
        {
            if (inventory.hotbarSlots[i].assignedItem == Material1.Item)
            {
                m1 = true;
                m1_loc = i;
            }
            if (inventory.hotbarSlots[i].assignedItem == Material2.Item)
            {
                m2 = true;
                m2_loc = i;
            }
            if(m1 && m2)
            {
                inventory.hotbarSlots[m1_loc].RemoveItem();
                inventory.hotbarSlots[m2_loc].RemoveItem();
                inventory.Add(CraftedItem.item);
            }
        }
    }
}
