using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The BossEnemyState script is a type of EnemyState, keeping track of boss enemies' states.
/// The speed, maximum health, current health, attack damage and boolean for isBoss are initialised.
/// </summary>
public class BossEnemyState : EnemyState
{
    /// <summary>
    /// Set the attributes for the boss enemies.
    /// </summary>
    void Start()
    {
        speed = 1.5f; // Boss movement speed.
        maxHealth = 150.0f; // Boss maximum health.
        health = maxHealth; // Initially set their current health to their max health.
        attackDamage = 20.0f; // Boss attack damage.
        isBoss = true; // Bosses are bosses. Used to check win condition.
    }
}
