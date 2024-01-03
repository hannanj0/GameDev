using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Add this namespace to use TextMeshPro

public class CrosshairScript : MonoBehaviour
{
    public Transform crosshair; // Assign your crosshair UI element here
    public Material highlightMaterial; // Assign a material for highlighting
    public float highlightRange = 1.0f; // Set the range within which items will be highlighted
    public TextMeshProUGUI itemLabel; // Assign your TextMeshPro UI element here

    private Material originalMaterial;
    private Transform lastHighlighted;

    void Update()
    {
        Vector3 crosshairWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(crosshair.position.x, crosshair.position.y, Camera.main.nearClipPlane));
        RaycastHit[] hits = Physics.SphereCastAll(crosshairWorldPosition, highlightRange, Camera.main.transform.forward, 100f);

        bool foundItem = false;
        
        foreach (var hit in hits)
        {
            Transform hitTransform = hit.transform;
            if (hitTransform.CompareTag("Item"))
            {
                if (!foundItem || Vector3.Distance(crosshairWorldPosition, hitTransform.position) < Vector3.Distance(crosshairWorldPosition, lastHighlighted.position))
                {
                    foundItem = true;
                    HighlightItem(hitTransform);
                }
            }
        }

        if (!foundItem && lastHighlighted != null)
        {
            ResetHighlight();
        }
    }

    private void HighlightItem(Transform item)
    {
        if (lastHighlighted != null && lastHighlighted != item)
        {
            ResetHighlight();
        }

        if (lastHighlighted != item)
        {
            originalMaterial = item.GetComponent<Renderer>().material;
            item.GetComponent<Renderer>().material = highlightMaterial;
            lastHighlighted = item;

            // Split the name by spaces and take the first part
            string itemName = item.name.Split(' ')[0];
            itemLabel.text = itemName; // Set the text of the TextMeshPro UI element to the item's name
            Debug.Log("Highlighted Item: " + itemName);
        }
    }


    private void ResetHighlight()
    {
        if (lastHighlighted != null)
        {
            lastHighlighted.GetComponent<Renderer>().material = originalMaterial;
            lastHighlighted = null;

            // Clear the text of the TextMeshPro UI element
            itemLabel.text = "";
        }
    }
}
