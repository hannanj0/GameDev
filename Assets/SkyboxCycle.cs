using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxCycle : MonoBehaviour
{
    public Material DaySkybox;
    public Material NightSkybox;
    public string current;

    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        current = "day";
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //timer += Time.deltaTime;
        //if (timer  >= 5.0 &&  current == "day")
        //{
        //    RenderSettings.skybox = NightSkybox;
        //    current = "night";
        //    timer = 0;
        //    Debug.Log("change to night");
        //}
        //if (timer >= 5.0 && current == "night")
        //{
        //    RenderSettings.skybox = DaySkybox;
        //    current = "day";
        //    timer = 0;
            
        //}
    }

    public void ChangeToDay()
    {
        RenderSettings.skybox = DaySkybox;
    }

    public void ChangeToNight()
    {
        RenderSettings.skybox = NightSkybox;
    }
}
