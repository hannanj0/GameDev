using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

/// <summary>
/// The CrosshairScript script is used to highlight items and interactables in the game.
/// </summary>
public class CrosshairScript : MonoBehaviour
{
    public Transform crosshair; 
    public Material highlightMaterial; 
    public float highlightRange = 1.0f; 
    public TextMeshProUGUI itemLabel; 

    private Material originalMaterial;
    private Transform lastHighlighted;
    public float maxHighlightDistance = 10.0f; 


    void Update()
    {
        Vector3 crosshairWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(crosshair.position.x, crosshair.position.y, Camera.main.nearClipPlane));
        RaycastHit[] hits = Physics.SphereCastAll(crosshairWorldPosition, highlightRange, Camera.main.transform.forward, 100f);

        bool foundItem = false;

        foreach (var hit in hits)
        {
            Transform hitTransform = hit.transform;
            float distanceToHit = Vector3.Distance(crosshairWorldPosition, hitTransform.position);

            if (distanceToHit <= maxHighlightDistance && (hitTransform.CompareTag("Item") || hitTransform.CompareTag("Interactable")))
            {
                if (!foundItem || distanceToHit < Vector3.Distance(crosshairWorldPosition, lastHighlighted.position))
                {
                    foundItem = true;
                    HighlightItem(hitTransform);
                }
            }
        }

        if (!foundItem)
        {
            if (lastHighlighted != null)
            {
                ResetHighlight();
            }
            itemLabel.text = ""; // Clear the text if no item or interactable is found
        }
    }


    private void HighlightItem(Transform item)
    {
        if (item.CompareTag("Interactable"))
        {
            // Only show the message for interactable objects
            itemLabel.text = "Press E to Interact";
            // Don't highlight the interactable object
            if (lastHighlighted != null && lastHighlighted != item)
            {
                ResetHighlight();
            }
            lastHighlighted = item; // Keep track of the last highlighted item
        }
        else if (item.CompareTag("Item"))
        {
            // Continue with the normal highlighting process for other items
            if (lastHighlighted != null && lastHighlighted != item)
            {
                ResetHighlight();
            }

            if (lastHighlighted != item)
            {
                originalMaterial = item.GetComponent<Renderer>().material;
                item.GetComponent<Renderer>().material = highlightMaterial;
                lastHighlighted = item;

                string itemName = item.name.Split(' ')[0];
                itemLabel.text = itemName;
                Debug.Log("Highlighted Item: " + itemName);
            }
        }
    }

    private void ResetHighlight()
    {
        if (lastHighlighted != null)
        {
            lastHighlighted.GetComponent<Renderer>().material = originalMaterial;
            lastHighlighted = null;
        }
    }
}