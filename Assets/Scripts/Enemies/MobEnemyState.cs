using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobEnemyState : EnemyState
{
    void Start()
    {
        speed = 3.0f;
        maxHealth = 100.0f;
        health = maxHealth;
        attackDamage = 10.0f;
        isBoss = false;
    }
}
