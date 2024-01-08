using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

/// <summary>
/// The PlayerInteractions script manages the player's interactions in the game.
/// It manages item usage, the inventory and taking damage.
/// </summary>
public class PlayerInteractions : MonoBehaviour
{
    public CloudSave cloudSave;

    public GameObject menuController;

    private bool inContact; 
    private bool craftingMenuOpen; 
    private float enemyAttackCooldown = 0.0f; 
    private float enemyAttackCooldownDuration = 1.5f; 
    private PlayerState playerState; 
    private EnemyState enemyState; 
    private Inventory inventory; 
    private PlayerControls playerControls;
    private InputAction useItemAction; 
    private InputAction openCMenu;

    private ItemDescription background;
    private GameSavedInfo popupInfo;
    private Canvas craftingTable;

    private string currentTerrain = "";
    public BackgroundAudioController gameAudio;


    public bool CraftingMenuOpen() {  return craftingMenuOpen; }

    void Awake()
    {
        // Initialize the PlayerControls
        playerControls = InputManager.Instance.Controls; 
        useItemAction = playerControls.Gameplay.UseItem; 
        openCMenu = playerControls.Gameplay.OpenCraftingMenu;
    }

    void Start()
    {
        // Initialise variables.
        inContact = false;
        craftingMenuOpen = false;

        Transform inventoryPlayer = transform.Find("Inventory");
        inventory = inventoryPlayer.GetComponent<Inventory>();
        inventory.SelectSlot(0);

        playerState = GetComponent<PlayerState>();

        Transform backgroundChild = transform.Find("ItemInformation/BackgroundColour");
        background = backgroundChild.GetComponent<ItemDescription>();
        Transform saveObject = transform.Find("ItemInformation/SaveGameDisplay");
        popupInfo = saveObject.GetComponent<GameSavedInfo>();


        Transform craftingPlayer = transform.Find("Crafting");
        craftingTable = craftingPlayer.GetComponent<Canvas>();
    }

    private void OnEnable()
    {

        useItemAction.performed += UseItem;
        useItemAction.Enable();

        openCMenu.performed += OpenCraftingMenu;
        openCMenu.Enable();
    }

    private void OnDisable()
    {
        useItemAction.performed -= UseItem;
        useItemAction.Disable();

        openCMenu.performed -= OpenCraftingMenu;
        openCMenu.Disable();
    }

    /// <summary>
    /// Update the timer and keep track of the player taking damage.
    /// The player is attacked every enemyAttackCooldownDuration if they are in contact with an enemy.
    /// </summary>
    void Update()
    {
            enemyAttackCooldown += Time.deltaTime;
            if (enemyAttackCooldown >= enemyAttackCooldownDuration)
            {
                if (inContact && enemyState.Health() > 0)
                {
                    playerState.TakeDamage(enemyState.AttackDamage());
                    enemyAttackCooldown = 0.0f;
                }
            }
    }
    /// <summary>
    /// Uses an item
    /// If the slot is not empty uses the item and then removes it
    /// </summary>
    private void UseItem(InputAction.CallbackContext context)
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
    /// <summary>
    /// Opens the crafting menu
    /// When the butten assigned to input action is pressed the canvas containing the crafting menu is enabled
    /// The cursor is made visiable and not locked and the game is paused
    /// When the button is pressed again the canvas becomes disabled and the cursor is locked and made invisible and the game resumes running
    /// </summary>
    private void OpenCraftingMenu(InputAction.CallbackContext context)
    {
        craftingTable.enabled = !craftingTable.enabled;
        if (craftingTable.enabled)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
            craftingMenuOpen = true;
            Time.timeScale = 0.0f;
        }
        else
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
            craftingMenuOpen = false;
            Time.timeScale = 1.0f;
        }
    }

    /// <summary>
    /// Registers a collision with the enemy, they are in contact so they can be attacked.
    /// </summary>
    /// <param name="other"> Other entity colliding with the player. </param>
    private void OnCollisionEnter(Collision other)
    {
        // Take damage from the enemy when the player can be attacked again.
        if (other.gameObject.CompareTag("Enemy") && !inContact)
        {
            enemyState = other.gameObject.GetComponent<EnemyState>();
            inContact = true;
        }
        if (other.gameObject.name != currentTerrain)
        {
            if (other.gameObject.name == "Forest Terrain")
            {
                currentTerrain = other.gameObject.name;
                gameAudio.SetInForestArea();
                Debug.Log("in forest area");
            }
            if (other.gameObject.name == "Arid Terrain")
            {
                currentTerrain = other.gameObject.name;
                gameAudio.SetInDesertArea();
                Debug.Log("in arid area");
            }
            if (other.gameObject.name == "Volcanic Terrain")
            {
                currentTerrain = other.gameObject.name;
                gameAudio.SetInVolcanicArea();
                Debug.Log("in volcanic area");
            }
            if (other.gameObject.name == "Spooky Terrain")
            {
                currentTerrain = other.gameObject.name;
                gameAudio.SetInSpookyArea();
                Debug.Log("in spooky area");
            }
        }
        // The player cannot be attacked for the AttackCooldownDuration since they just got attacked.
    }

    /// <summary>
    /// Registers a collision exit with the enemy. They are no longer in contact.
    /// </summary>
    /// <param name="other"> Other entity colliding with the player. </param>
    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            inContact = false;
        }
    }
    /// <summary>
    /// Used to pick up an item
    /// If the object has the item tag it is picked up
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        //
        if (other.gameObject.CompareTag("Item"))
        {
            GameItem I = other.gameObject.GetComponent<GameItem>();
            inventory.Add(I.item);
            other.gameObject.SetActive(false);

            background.DisplayDescription(I);
        }

        else if (other.tag == "Checkpoint")
        {
            playerState.spawnPosition = other.transform.position;
            playerState.spawnPosition.y = 0.5f;

            cloudSave.SaveGame();
            popupInfo.DisplayDescription();
        }

        else if (other.tag == "Idol")
        {
            playerState.IncrementIdols();
            other.gameObject.SetActive(false);

        }
    }

    private void OnDestroy()
    {
        // Clean up and disable the action when this object is destroyed
        useItemAction.performed -= UseItem;
        useItemAction.Disable();
    }

    private void OnPause()
    {
        menuController.GetComponent<PauseMenu>().OnPause();
    }
}
