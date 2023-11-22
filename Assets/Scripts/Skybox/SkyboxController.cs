using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    private Material skybox;
    public float speed;

    public float dayNightTint;

    // Start is called before the first frame update
    void Start()
    {
        skybox = RenderSettings.skybox;
        speed = 0.01f;

        dayNightTint = 0.6f;
    }

    // Update is called once per frame
    void Update()
    {
        skybox.SetFloat("_Rotation", skybox.GetFloat("_Rotation") + Time.deltaTime * speed);

        skybox.SetColor("_Tint", new Color(dayNightTint, dayNightTint, dayNightTint, 1));


    }
}
