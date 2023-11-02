using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    protected float speed;
    protected float maxHealth;
    protected float health;
    protected float attackDamage;
    protected Boolean isBoss;

    [SerializeField] EnemyHealthBar healthBar;

    public float Speed() { return speed; }
    public float MaxHealth() { return maxHealth;}
    public float Health() { return health;}
    public float AttackDamage() {  return attackDamage;}
    public bool IsBoss() { return isBoss;}

    public void TakeDamage(float damage) 
    {
        health -= damage;
    }

    private void Awake()
    {
        healthBar = GetComponentInChildren<EnemyHealthBar>();
    }
}