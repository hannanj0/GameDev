using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Create Food")]
public class Food : Item
{
    public float satiation;
    /// <summary>
    /// Feeds the player equal to satiation 
    /// Ensures that the hunger value does not go above maxHunger
    /// </summary>
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
