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
    
    public void HandleGlobalSlider (float target) 
    {
        Audio audio = FindObjectOfType<Audio>();
        PlayerPrefs.SetFloat("globalFloat", target);
        audio.ChangeGlobalVolume(target);
    }

    public void HandleEffectsSlider (float target) 
    {
        Audio audio = FindObjectOfType<Audio>();
        PlayerPrefs.SetFloat("effectFloat", target);
        audio.ChangeSoundEffectsVolume(target);
    }
    

    public void HandleMusicSlider (float target) 
    {
        Audio audio = FindObjectOfType<Audio>();
        PlayerPrefs.SetFloat("musicFloat", target);
        audio.ChangeMusicVolume(target);
    }
    
    
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
}
