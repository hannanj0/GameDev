using System.Collections;
using TMPro;
using UnityEngine;

public class Conversation : MonoBehaviour
{
    [SerializeField] private Canvas dialogueCanvas;
    [SerializeField] private TextMeshProUGUI dialogueText;

    private void Start()
    {
        // Ensure the conversation is disabled initially
        dialogueCanvas.enabled = false;
    }

    public void StartConversation(string[] dialogue)
    {
        StartCoroutine(DisplayDialogue(dialogue));
    }

    private IEnumerator DisplayDialogue(string[] dialogue)
    {
        dialogueCanvas.enabled = true;

        foreach (string line in dialogue)
        {
            dialogueText.text = line;

            // Wait for some time, adjust the duration as needed
            yield return new WaitForSeconds(2f);
        }

        dialogueCanvas.enabled = false;
    }
}
