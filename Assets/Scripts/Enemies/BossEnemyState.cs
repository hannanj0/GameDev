using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemyState : EnemyState
{
    void Start()
    {
        speed = 1.5f;
        maxHealth = 150.0f;
        health = maxHealth;
        attackDamage = 20.0f;
        isBoss = true;
    }
}
