using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public PauseMenu pauseMenu;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position;
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (!pauseMenu.GameIsPaused())
        {
            transform.position = player.transform.position + offset;
        }
    }
}
