using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public int damage = 10;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerState playerState = other.GetComponent<PlayerState>();

            if (playerState != null)
            {
                playerState.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
