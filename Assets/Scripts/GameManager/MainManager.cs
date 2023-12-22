using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public CloudSave cloudSave;
    PlayerData playerData;

    

    private void Start()
    {

        if (GameManager.Instance != null && GameManager.Instance.loadGameRequest)
        {
            playerData = GameManager.Instance.playerData;   
            
        }

    }

    public PlayerData LoadPlayerData()
    {
        return playerData;
    }
}
