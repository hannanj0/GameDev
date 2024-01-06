using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is the script for the bullet used by the enemy that shoots
/// </summary>

public class BulletScript : MonoBehaviour
{
    public int damage = 10; // Damage the bullet can inflict, can be changed in the Unity inspector

    /// <summary>
    /// This is the functionality of the bullet, checking if the bullet touches a game object with the tag "Player" (the user itself), and then causes them to take damage and then destroying the bullet object itself upon collision
    /// </summary>
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerState playerState = other.GetComponent<PlayerState>();

            if (playerState != null)
            {
                playerState.TakeDamage(damage); // Reference to the PlayerState script
            }

            Destroy(gameObject);
        }
    }
}
