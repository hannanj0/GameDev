using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent enemy;
    public Transform player;

    public float timer = 5;
    private float bulletTime;

    public GameObject enemyBullet;
    public Transform spawnPoint;
    public float enemySpeed;

    private GameObject bulletObj; // Declare bulletObj at the class level

    void Start()
    {
        bulletTime = timer;
    }

    void Update()
    {
        enemy.SetDestination(player.position);
        ShootAtPlayer();
    }

    // Method to initiate shooting
    public void StartShooting()
    {
        // Assign the instantiated bullet object to the class-level variable
        bulletObj = Instantiate(enemyBullet, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        bulletRig.AddForce(bulletRig.transform.forward * enemySpeed);
        Destroy(bulletObj, 5f);
    }

    // Make ShootAtPlayer method public
    public void ShootAtPlayer()
    {
        bulletTime -= Time.deltaTime;

        if (bulletTime > 0) return;

        bulletTime = timer;

        // Assign the instantiated bullet object to the class-level variable
        bulletObj = Instantiate(enemyBullet, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        bulletRig.AddForce(bulletRig.transform.forward * enemySpeed);
        Destroy(bulletObj, 5f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(bulletObj);
        }
    }
}
