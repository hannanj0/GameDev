using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The MobEnemyState script is a type of EnemyState, keeping track of mob enemies' stats.
/// </summary>
public class MobEnemyState : EnemyState
{
    /// <summary>
    /// Set the attributes for the mob enemies.
    /// </summary>
    void Start()
    {
        speed = 3.0f; // Mob movement speed.
        maxHealth = 100.0f;  // Mob maximum health.
        health = maxHealth;  // Set the current health to the max health when enemies are initialised.
        attackDamage = 10.0f; // Mob attack damage.
        isBoss = false; // Mobs are not bosses.       
    }
}
