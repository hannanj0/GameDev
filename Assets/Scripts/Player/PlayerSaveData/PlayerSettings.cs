using System;
using System.Collections.Generic;

[System.Serializable]
public class PlayerSettings
{
    public bool fullScreen = false;
    public int gameDifficulty = 1;
    public float sensitivity = 5f;
    public int graphicsSetting = 1;
    public float masterVolume = 0f;
    public float musicVolume = 0f;
    public float sfxVolume = 0f;
    public bool isMuted = false;

}
