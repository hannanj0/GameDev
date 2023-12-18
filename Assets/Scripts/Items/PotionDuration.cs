using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionDuration : MonoBehaviour
{
    private float time;
    private bool active = false;
    private PlayerState ps_loc;
    private float strength;
    // Update is called once per frame
    void Update()
    {
        if(active){
            if(time > 0){
                time -= Time.deltaTime;
                Debug.Log(time);
            }
            else{
                Debug.Log("done");
                active = false;
                ps_loc.IncreaseDamage(-strength);
            }
        }
    }
    public void startTimer(float duration, float s, PlayerState ps)
    {
        strength = s;
        ps_loc = ps;
        time = duration;
        active = true;
    }
}
