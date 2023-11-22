using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public bool playerInRange;
    public GameObject npcDialogueCanvas; // Reference to the NPCDialogue canvas

    private void Start()
    {
        // Deactivate the NPCDialogue canvas when the game starts
        npcDialogueCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            ShowDialogueCanvas();
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            HideDialogueCanvas();
        }
    }

    // Show the NPCDialogue canvas
    private void ShowDialogueCanvas()
    {
        npcDialogueCanvas.SetActive(true);
    }

    // Hide the NPCDialogue canvas
    private void HideDialogueCanvas()
    {
        npcDialogueCanvas.SetActive(false);
    }
}
