using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Create Food")]
public class Food : Item
{
    public float satiation;

    public override void Use (PlayerState ps)
    {
        if (ps.currentHunger == ps.maxHunger)
        {
            return;
        }
        else if (ps.currentHunger + satiation > ps.maxHunger)
        {
            Debug.Log(ps.currentHunger);
            ps.currentHunger = ps.maxHunger;
        }
        else
        {
            ps.currentHunger += satiation;
        }
    }
}
