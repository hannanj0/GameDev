using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The MobEnemyState script is a type of EnemyState, keeping track of mob enemies' states.
/// The speed, maximum health, current health, attack damage and boolean for isBoss are initialised.
/// </summary>
public class MobEnemyState : EnemyState
{
    /// <summary>
    /// Set the attributes for the mob enemies.
    /// </summary>
    void Start()
    {
        speed = 3.0f;      
        maxHealth = 100.0f;  
        health = maxHealth;  
        attackDamage = 10.0f;
        isBoss = false;       
    }
}
