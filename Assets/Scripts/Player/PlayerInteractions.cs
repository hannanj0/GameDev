using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    GameObject player;
    public PlayerState playerState;

    private float enemyCollisionCooldown = 0.0f;
    private float enemyCollisionCooldownDuration = 2.0f;
    private bool offCooldown;
    private Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        offCooldown = true;
        Transform inventoryPlayer = transform.Find("Inventory");
        playerState = GetComponent<PlayerState>();
        if (inventoryPlayer != null) { inventory = inventoryPlayer.GetComponent<Inventory>(); }

        if (inventoryPlayer == null) { Debug.LogError("No inventory"); }
    }

    // Update is called once per frame
    void Update()
    {
        if (!offCooldown)
        {
            enemyCollisionCooldown += Time.deltaTime;
            if (enemyCollisionCooldown >= enemyCollisionCooldownDuration)
            {
                offCooldown = true;
                enemyCollisionCooldown = 0.0f;
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (inventory.hotbarSlots[inventory.currentSlot] != null)
            {
                inventory.hotbarSlots[inventory.currentSlot].assignedItem.Use(playerState);
                inventory.hotbarSlots[inventory.currentSlot].RemoveItem();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && offCooldown) {
            EnemyState enemy = other.gameObject.GetComponent<EnemyState>();
            playerState.currentHealth -= enemy.attackDamage;

            offCooldown = false;
            enemyCollisionCooldown = 0.0f;
        }
        if (other.gameObject.CompareTag("Item"))
        {
            GameItem I = other.GetComponent<GameItem>();
            Debug.Log("Item Picked Up");
            inventory.Add(I.item);
            other.gameObject.SetActive(false);
        }
    }
}
