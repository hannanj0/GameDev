using System.Collections;
using UnityEngine;

public class BackgroundAudioController : MonoBehaviour
{
    private AudioSource[] gameAudio;
    private AudioSource currentSFX;

    private bool inForestArea;
    private bool inDesertArea;
    private bool inVolcanicArea;
    private bool inSpookyArea;

    private bool fadingIn;
    private bool fadingOut;

    private float fadeSpeed = 0.15f;
    private float currentMaxVolume = 0.3f;
    private float forestMaxVolume = 0.3f;
    private float desertMaxVolume = 0.06f;
    private float volcanicMaxVolume = 0.2f;
    private float spookyMaxVolume = 0.15f;
    private bool reassignSFX = false;

    public void SetInForestArea()
    {
        inForestArea = true;
        inDesertArea = false;
        inVolcanicArea = false;
        inSpookyArea = false;
        reassignSFX = true;
    }

    public void SetInDesertArea()
    {
        inForestArea = false;
        inDesertArea = true;
        inVolcanicArea = false;
        inSpookyArea = false;
        reassignSFX = true;
    }

    public void SetInVolcanicArea()
    {
        inForestArea = false;
        inDesertArea = false;
        inVolcanicArea = true;
        inSpookyArea = false;
        reassignSFX = true;
    }

    public void SetInSpookyArea()
    {
        inForestArea = false;
        inDesertArea = false;
        inVolcanicArea = false;
        inSpookyArea = true;
        reassignSFX = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        inForestArea = false;
        inVolcanicArea = false;
        inDesertArea = false;
        inSpookyArea = false;
        gameAudio = GetComponents<AudioSource>();
        currentSFX = gameAudio[0];
    }

    void Update()
    {
        // Terrain changes
        if (inForestArea && reassignSFX)
        {
            StartCoroutine(FadeOutAndReassign(gameAudio[0], forestMaxVolume));
            reassignSFX = false;
        }
        if (inDesertArea && reassignSFX)
        {
            StartCoroutine(FadeOutAndReassign(gameAudio[1], desertMaxVolume));
            reassignSFX = false;
        }
        if (inVolcanicArea && reassignSFX)
        {
            StartCoroutine(FadeOutAndReassign(gameAudio[2], volcanicMaxVolume));
            reassignSFX = false;
        }
        if (inSpookyArea && reassignSFX)
        {
            StartCoroutine(FadeOutAndReassign(gameAudio[3], spookyMaxVolume));
            reassignSFX = false;
        }

        // Fading out
        if (currentSFX.volume > 0 && fadingOut)
        {
            currentSFX.volume -= fadeSpeed * Time.deltaTime;
        }

        // Faded out
        if (currentSFX.volume <= 0 && fadingOut)
        {
            fadingOut = false;
            currentSFX.Stop();
        }

        // Fading in
        if (currentSFX.volume < currentMaxVolume && fadingIn)
        {
            currentSFX.volume += fadeSpeed * Time.deltaTime;
        }

        // Faded in
        if (currentSFX.volume >= currentMaxVolume && fadingIn)
        {
            fadingIn = false;
        }
    }

    private IEnumerator FadeOutAndReassign(AudioSource newSFX, float newMaxVolume)
    {
        fadingOut = true;

        // Smoothly fade out the current sound effect
        while (currentSFX.volume > 0)
        {
            currentSFX.volume -= fadeSpeed * Time.deltaTime;
            yield return null;
        }

        // After fading out, assign the new terrain sound effect
        currentSFX.Stop();
        currentSFX = newSFX;
        currentMaxVolume = newMaxVolume;
        currentSFX.volume = 0;
        currentSFX.Play();
        fadingIn = true;
    }

    public void StartFadeOut()
    {
        if (!fadingOut)
        {
            StartCoroutine(FadeOut());
        }
    }

    // Coroutine to fade out the current sound effect
    private IEnumerator FadeOut()
    {
        fadingOut = true;

        while (currentSFX.volume > 0)
        {
            currentSFX.volume -= fadeSpeed * Time.deltaTime;
            yield return null;
        }

        fadingOut = false;
        currentSFX.Stop();
    }
}

