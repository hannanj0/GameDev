using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Backup : MonoBehaviour
{
    GameObject player;
    private PlayerState playerState;
    private EnemyState enemyState;

    private float damageTakenCooldown = 2.0f;
    private float timeSinceAttacked = 0.0f;
    private bool isBeingAttacked;
    private Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        isBeingAttacked = false;
        Transform inventoryPlayer = transform.Find("Inventory");
        playerState = GetComponent<PlayerState>();
        if (inventoryPlayer != null) { inventory = inventoryPlayer.GetComponent<Inventory>(); }

        if (inventoryPlayer == null) { Debug.LogError("No inventory"); }
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingAttacked)
        {
            timeSinceAttacked += Time.deltaTime;
            if (timeSinceAttacked >= damageTakenCooldown)
            {
                playerState.currentHealth -= enemyState.AttackDamage();
                timeSinceAttacked = 0.0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (!inventory.hotbarSlots[inventory.currentSlot].isEmpty)
            {
                if (inventory.hotbarSlots[inventory.currentSlot].assignedItem.consumable)
                {
                    inventory.hotbarSlots[inventory.currentSlot].assignedItem.Use(playerState);
                    inventory.hotbarSlots[inventory.currentSlot].RemoveItem();
                }

            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            GameItem I = other.GetComponent<GameItem>();
            Debug.Log("Item Picked Up");
            inventory.Add(I.item);
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            enemyState = other.gameObject.GetComponent<EnemyState>();
            isBeingAttacked = true;
            playerState.currentHealth -= enemyState.AttackDamage();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            isBeingAttacked = false;
            timeSinceAttacked = 0.0f;
        }
    }
}