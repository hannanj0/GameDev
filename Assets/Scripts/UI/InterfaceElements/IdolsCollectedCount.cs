using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // might not need this
using TMPro;

public class IdolsCollectedCount : MonoBehaviour
{
    public TextMeshProUGUI collectedIdols;
    public PlayerState playerState;

    private int idolsCollected, totalIdols;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        idolsCollected = playerState.IdolsCollectedCount();
        totalIdols = playerState.TotalIdols();
        collectedIdols.text = "Idols Collected: " + idolsCollected + "/" + totalIdols;
    }
}

