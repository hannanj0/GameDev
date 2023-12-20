using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Create StrengthPotion")]
public class StrengthPotion : Item
{
    public float strength;
    public float potionLength;
    //public PotionDuration pd;
    /// <summary>
    /// Increases the attack damage of the player by the value assigned to strength
    /// </summary>
    public override void Use(PlayerState ps)
    {
        ps.IncreaseDamage(strength);
        //pd.startTimer(potionLength, strength, ps);
    }
}