using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    private static string volumePref = "volumePref"; // initializnign to 


    public Slider globalSlider, musicSlider, soundEffectSlider;

    public AudioMixer mixer;
    
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("globalFloat"))
        {
            globalSlider.value = PlayerPrefs.GetFloat("globalFloat");
            mixer.SetFloat("MasterVolume", PlayerPrefs.GetFloat("globalFloat"));
        }
        else
        {
            globalSlider.value = -6f;
            mixer.SetFloat("MasterVolume", -6f);
        }

        if (PlayerPrefs.HasKey("effectFloat"))
        {
            soundEffectSlider.value = PlayerPrefs.GetFloat("effectFloat");
            mixer.SetFloat("SoundEffectsVolume", PlayerPrefs.GetFloat("effectFloat"));
        }
        else
        {
            soundEffectSlider.value = -6f;
            mixer.SetFloat("SoundEffectsVolume", -6f);
        }

        if (PlayerPrefs.HasKey("musicFloat"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicFloat");
            mixer.SetFloat("MusicVolume", PlayerPrefs.GetFloat("musicFloat"));
        }
        else
        {
            musicSlider.value = -6f;
            mixer.SetFloat("MusicVolume", -6f);
        }
    }

    public void SaveGlobalVolumeLevel(float volume)
    {
        PlayerPrefs.SetFloat("globalFloat", volume);
    }

    public void SaveMusicLevel(float volume)
    {
        PlayerPrefs.SetFloat("musicFloat", volume);
    }
    
    public void SaveEffectLevel(float volume)
    {
        PlayerPrefs.SetFloat("effectFloat", volume);
    }
}
