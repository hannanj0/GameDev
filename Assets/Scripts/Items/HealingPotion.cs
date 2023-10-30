using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Create HealingPotion")]
public class HealingPotion : Item
{
    public int healingAmount = 20;
    public override void Use(PlayerState ps)
    {
        if (ps.currentHealth == ps.maxHealth)
        {
            return;
        }
        else if (ps.currentHealth + healingAmount > ps.maxHealth)
        {
            Debug.Log(ps.currentHealth);
            ps.currentHealth = ps.maxHealth;
        }
        else
        {
            ps.currentHealth += healingAmount;
        }
    }
}
