using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Audio : MonoBehaviour
{
    public AudioSource stationRadio;
    public AudioSource trainRadio;
    public AudioSource departureRadio;
    public AudioSource arrivalRadio;
    public AudioSource ambientRadio;

    public float fadeDuration;

    void Start ()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadType)
    {
        if (scene.name == "Station" && stationRadio.volume == 0)
        {
            ArriveAtStation();
        }
        else if (scene.name == "trainplace")
        {
            DepartStation();
        }
        else if (scene.name == "MainMenu")
        {
            ArriveAtStation();
        }
    }

    void Update ()
    {
        if (!departureRadio.isPlaying && stationRadio.volume == 0 && !ambientRadio.isPlaying) 
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