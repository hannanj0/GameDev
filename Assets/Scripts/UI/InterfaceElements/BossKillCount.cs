using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // might not need this
using TMPro;

public class BossKillCount : MonoBehaviour
{
    public TextMeshProUGUI bossesSlain;
    public PlayerState playerState;

    private int bossesKilled, totalGameBosses;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bossesKilled = playerState.BossesKilledCount();
        totalGameBosses = playerState.TotalGameBosses();
        bossesSlain.text = "Bosses Slain: " + bossesKilled + "/" + totalGameBosses;
    }
}
