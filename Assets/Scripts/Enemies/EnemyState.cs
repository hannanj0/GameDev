using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    public float speed;
    public float health;
    public float attackDamage;

    public float EnemySpeed
    {
        get { return speed; }
        set { speed = value; }
    }

    public float EnemyHealth
    {
        get { return health; }
        set { health = value; }
    }

    public float EnemyAttackDamage
    {
        get { return attackDamage; }
        set { attackDamage = value; }
    }

    public Transform[] patrolLocations;
    public int targetLocation = 0;

    // Start is called before the first frame update
    void Start()
    {
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
