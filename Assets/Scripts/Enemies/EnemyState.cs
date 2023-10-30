using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public float speed;
    public float maxHealth;
    public float health;
    public float attackDamage;
    public Boolean isBoss;

    public Transform[] patrolLocations;
    public int targetLocation = 0;

    [SerializeField] EnemyHealthBar healthBar;

    private void Awake()
    {
        healthBar = GetComponentInChildren<EnemyHealthBar>();
    }



    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 100.0f;
        health = maxHealth;
        transform.position = patrolLocations[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, patrolLocations[targetLocation].position, speed * Time.deltaTime);
        transform.LookAt(patrolLocations[targetLocation]);

        if (transform.position == patrolLocations[targetLocation].position && targetLocation == 0)
        {
            targetLocation = 1;
        }
        else if (transform.position == patrolLocations[targetLocation].position && targetLocation == 1)
        {
            targetLocation = 0;
        }

    }
}
