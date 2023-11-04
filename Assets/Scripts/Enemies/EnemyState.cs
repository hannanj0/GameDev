using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The EnemyState script defines all enemy states (mob and boss enemies). 
/// Speed, maximum health, current health, attack damage and boolean isBoss attributes can be set and accessed.
/// The enemy is able to take damage.
/// </summary>
public class EnemyState : MonoBehaviour
{
    protected float speed; // Enemy speed.
    protected float maxHealth; // Enemy maximum health.
    protected float health; // Enemy current health.
    protected float attackDamage; // Enemy attack damage.
    protected Boolean isBoss; // Check if enemy is a boss.

    [SerializeField] EnemyHealthBar healthBar; // Enemy health bar UI

    // Getter methods for attributes
    public float Speed() { return speed; }
    public float MaxHealth() { return maxHealth;}
    public float Health() { return health;}
    public float AttackDamage() {  return attackDamage;}
    public bool IsBoss() { return isBoss;}

    /// <summary>
    /// Reduces the enemy's health by the given (player's) attack damage.
    /// </summary>
    public void TakeDamage(float damage) 
    {
        health -= damage;
    }

    /// <summary>
    /// Find and initialise the Health Bar UI.
    /// </summary>
    private void Awake()
    {
        healthBar = GetComponentInChildren<EnemyHealthBar>();
    }
}