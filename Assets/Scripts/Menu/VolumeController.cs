using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    private static string volumePref = "volumePref"; // initializnign to 


    public Slider globalSlider, musicSlider, soundEffectSlider;
    private float volumeFloat = .25f, soundEffectFloat = .75f;

    // Start is called before the first frame update
    void Awake()
    {

        PlayerPrefs.SetFloat(volumePref, volumeFloat);
        if(PlayerPrefs.GetFloat("volumeFloat") == 0 || PlayerPrefs.GetFloat("soundEffectFloat") == 0)
        {
            PlayerPrefs.SetFloat("volumeFloat", volumeFloat);
            PlayerPrefs.SetFloat("soundEffectFloat", soundEffectFloat);
        }
    }

    public void SaveVolumeLevel(float volume)
    {
        PlayerPrefs.SetFloat("volumeFloat", volume);
    }
    
    public void SaveEffectLevel(float volume)
    {
        PlayerPrefs.SetFloat("soundEffectFloat", volume);
    }
}
