using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool loadGameRequest = false;
    public bool loadSettingsRequest = false;
    public bool inGame = false;
    public int gameDifficulty = 1;

    public PlayerData playerData;

    public PlayerSettings playerSettings;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

