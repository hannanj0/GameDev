using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the script enemy that shoots, which outlines that when in chase mode, it will shoot the bullets at the player, from a specific spawn point
/// </summary>

public class EnemyShoot : MonoBehaviour
{
    /// <summary>
    /// The public variables accessible through the Unity Inspector
    /// </summary>
    public UnityEngine.AI.NavMeshAgent enemy;
    public Transform player;

    public float timer = 5;
    private float bulletTime;

    public GameObject enemyBullet;
    public Transform spawnPoint;
    public float enemySpeed;

    private GameObject bulletObj;
    private bool isPatrolling = true;
    public AudioSource LavaMinionAttack;

    void Start()
    {
        bulletTime = timer; // this initialises the bullet with a timer value
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
            enemy.SetDestination(player.position); // Sets the destination to the player's position
            ShootAtPlayer(); // Initiates the shooting
            LavaMinionAttack.Play();
        }
    }

    /// <summary>
    /// This is what happens when the enemy begins shooting, instantiating, disabling, setting the initial velocity and a timer for when the object will be destroyed to prevent it from staying in the scene
    /// </summary>
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

    /// <summary>
    /// Coroutine to enable the bullet and destory it after a delay
    /// </summary>
    private IEnumerator EnableAndDestroyBullet(GameObject bullet, float delay)
    {  
        yield return new WaitForSeconds(delay);

        // Enable the bullet before destroying it
        bullet.SetActive(true);

        // Destroy the bullet after it has been enabled
        Destroy(bullet, 0.1f); // You can adjust the delay here
    }



    /// <summary>
    /// This is used for when the enemy is shooting at the player specifically, using checks and again destroying the object after a certain time
    /// </summary>
    public void ShootAtPlayer()
    {
        bulletTime -= Time.deltaTime;

        if (bulletTime <= 0)
        {
            bulletTime = timer;

            // Only instantiate a new bullet if the previous one doesn't exist anymore.
            if (bulletObj == null)
            {
                bulletObj = Instantiate(enemyBullet, spawnPoint.transform.position, spawnPoint.transform.rotation) as GameObject;
                Rigidbody bulletRig = bulletObj.GetComponent<Rigidbody>();
                bulletRig.AddForce(bulletRig.transform.forward * enemySpeed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPatrolling = false;
            // Check if the bulletObj is not null before attempting to destroy it.
            if (bulletObj != null)
            {
                Destroy(bulletObj);
                bulletObj = null; // Set bulletObj to null after destroying the object.
            }
        }
    }


}
