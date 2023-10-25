using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Create StrengthPotion")]
public class StrengthPotion : Item
{
    public float strength;

    public override void Use(PlayerState ps)
    {
        ps.attackDamage += strength;
    }
}
