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
    private bool isPatrolling = true;

    void Start()
    {
        bulletTime = timer;
    }

    void Update()
    {
        if (isPatrolling)  // Fix: Check if the enemy is patrolling
        {
            // Your patrolling logic here
            // For example, you can make the enemy move between waypoints
            // You might want to use a coroutine or other method for patrolling
            // For simplicity, I'm using a Debug.Log statement here
            // Debug.Log("Patrolling...");
        }
        else
        {
            enemy.SetDestination(player.position);
            ShootAtPlayer();
        }
    }

    public void StartShooting()
    {
        // Calculate the direction from the spawnPoint to the player
        Vector3 shootDirection = (player.position - spawnPoint.position).normalized;

        // Instantiate the bullet at the spawnPoint's position and rotation
        bulletObj = Instantiate(enemyBullet, spawnPoint.position, spawnPoint.rotation);

        // Disable the bullet prefab in the scene (if it's enabled by default)
        bulletObj.SetActive(false);

        // Set the bullet's initial velocity in the calculated direction
        Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
        bulletRig.velocity = shootDirection * enemySpeed;

        // Set a timer to enable the bullet and destroy it after a certain time
        StartCoroutine(EnableAndDestroyBullet(bulletObj, 5f));
    }

    // Coroutine to enable the bullet and destroy it after a delay
    private IEnumerator EnableAndDestroyBullet(GameObject bullet, float delay)
    {  
        yield return new WaitForSeconds(delay);

        // Enable the bullet before destroying it
        bullet.SetActive(true);

        // Destroy the bullet after it has been enabled
        Destroy(bullet, 0.1f); // You can adjust the delay here
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
            isPatrolling = false;
            Destroy(bulletObj);
        }
    }
}
