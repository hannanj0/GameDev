using UnityEngine;
/// <summary>
/// The GameManager script is used to keep track of the game state, and to keep track of the player's data and settings.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool loadGameRequest = false;
    public bool loadSettingsRequest = false;
    public bool inGame = false;
    public int gameDifficulty = 1;
    public bool signedIn = false;

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

