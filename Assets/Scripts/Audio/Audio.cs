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

    private bool ded = false;

    public void Die ()
    {
        ded = true;
        ambientRadio.Stop();
        trainRadio.Stop();
        stationRadio.Stop();
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
        else if (scene.name == "MainMenu")
        {
            ArriveAtStation();
        }
    }

    void Update ()
    {
        if (!departureRadio.isPlaying && stationRadio.volume == 0 && !ambientRadio.isPlaying && !ded) 
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
