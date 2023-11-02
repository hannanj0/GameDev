using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> inventory = new List<Item>();
    public HotbarSlot[] hotbarSlots;
    public int currentSlot = 0;
    public Item currentItem;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)){ SelectSlot(0);}
        else if (Input.GetKeyDown(KeyCode.Alpha2)) { SelectSlot(1); }
        else if (Input.GetKeyDown(KeyCode.Alpha3)) { SelectSlot(2); }
        else if (Input.GetKeyDown(KeyCode.Alpha4)) { SelectSlot(3); }

    }
    public void Add(Item item)
    {
        
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            HotbarSlot slot = hotbarSlots[i];
            if(slot.isEmpty)
            {
                inventory.Add(item);
                slot.AssignItem(item);
                break;
            }
        }
    }
    void SelectSlot (int newSlot)
    {
        hotbarSlots[currentSlot].Deselect();
        hotbarSlots[newSlot].Select();
        currentSlot = newSlot;
    }

}
