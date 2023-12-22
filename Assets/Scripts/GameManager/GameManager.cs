using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool loadGameRequest = false;

    public PlayerData playerData;

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

