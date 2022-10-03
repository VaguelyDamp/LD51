using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Audio : MonoBehaviour
{
    public AudioSource stationRadio;
    public AudioSource trainRadio;
    public AudioSource departureRadio;
    public AudioSource arrivalRadio;
    public AudioSource ambientRadio;
    public AudioSource winRadio;
    public AudioSource startGameRadio;

    public AudioMixer mixer;
    public string globalVolume = "MasterVolume";
    public string musicVolume = "MusicVolume";
    public string soundEffectsVolume = "SoundEffectsVolume";

    public float fadeDuration;

    private bool ded = false;

    public void Die ()
    {
        ded = true;
        ambientRadio.Stop();
        Fade(trainRadio, 0);
        Fade(stationRadio, 0);
        arrivalRadio.Stop();
        departureRadio.Stop();
    }

    void Awake ()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Audio");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        if (SceneManager.GetActiveScene().name == "trainplace")
        {
            DepartStation();
        }
    }

    public void StartGame ()
    {
        startGameRadio.Play();
    }

    public void ChangeGlobalVolume (float target)
    {
        if (target == -64f) target = -128f;
        mixer.SetFloat(globalVolume, target);
    }

    public void ChangeSoundEffectsVolume (float target)
    {
        if (target == -64f) target = -128f;
        mixer.SetFloat(soundEffectsVolume, target);
    }

    public void ChangeMusicVolume (float target)
    {
        if (target == -64f) target = -128f;
        mixer.SetFloat(musicVolume, target);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadType)
    {
        ded = false;
        if (scene.name == "Station" && stationRadio.volume == 0)
        {
            ArriveAtStation();
        }
        else if (scene.name == "trainplace")
        {
            DepartStation();
        }
        else if (scene.name == "MainMenu" && trainRadio.isPlaying)
        {
            Fade(stationRadio, 1);
        }
        else if (scene.name == "Winsville")
        {
            Die();
            winRadio.Play();
        }
    }

    void Update ()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            ambientRadio.Stop();
        }
        else if (!departureRadio.isPlaying && stationRadio.volume == 0 && !ambientRadio.isPlaying && !ded) 
        {
            ambientRadio.Play();
        }
        else if (arrivalRadio.isPlaying && ambientRadio.isPlaying) 
        {
            ambientRadio.Stop();
        }
    }

     public void ArriveAtStation ()
    {
        StartCoroutine(DoArriveAtStation());
    }

    private IEnumerator DoArriveAtStation ()
    {
        Fade(trainRadio, 0);
        arrivalRadio.Play();
        yield return new WaitForSeconds(fadeDuration);
        trainRadio.Stop();
        Fade(stationRadio, 1);
    }

    public void DepartStation ()
    {
        StartCoroutine(DoDepartStation());
    }

    private IEnumerator DoDepartStation ()
    {
        Fade(stationRadio, 0);
        departureRadio.Play();
        yield return new WaitForSeconds(fadeDuration);
        trainRadio.Play();
        Fade(trainRadio, 1);
    }

    void Fade (AudioSource source, int targetVolume) 
    {
        StartCoroutine(StartFade(source, fadeDuration, targetVolume));
    }

    private static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

}
