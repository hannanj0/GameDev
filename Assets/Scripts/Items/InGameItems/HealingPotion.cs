using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Create HealingPotion")]
public class HealingPotion : Item
{
    public float healingAmount;
    /// <summary>
    /// Heals the player equal to healingAmount
    /// Ensures that the healing does not go over maxHealth
    /// </summary>
    public override void Use(PlayerState ps)
    {
        if (ps.currentHealth == ps.maxHealth)
        {
            return;
        }
        else if (ps.currentHealth + healingAmount > ps.maxHealth)
        {
            ps.currentHealth = ps.maxHealth;
        }
        else
        {
            ps.currentHealth += healingAmount;
        }
    }
}
