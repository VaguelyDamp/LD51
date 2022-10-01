using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    public string nextSceneName;
    public float fadeInTime = 1.0f;
    public float fadeOutTime = 1.0f;
    
    private UnityEngine.UI.Image fadeOutImage;
    public GameObject fadeOutPrefab;

    private void Start()
    {
        GameObject fadeOutObj = GameObject.Instantiate(fadeOutPrefab, transform);
        fadeOutImage = fadeOutObj.GetComponent<UnityEngine.UI.Image>();

        StartCoroutine(SceneFadeIn());
    }

    public void StartSceneTransition() {
        StartCoroutine(SceneFadeOut(nextSceneName));
    }

    public void StartSceneTransition(string sceneName) {
        StartCoroutine(SceneFadeOut(sceneName));
    }

    private IEnumerator SceneFadeIn() {
        float timer = fadeInTime;

        while(timer > 0) {
            float t = 1 - (timer / fadeInTime);

            Color newColor = fadeOutImage.color;
            newColor.a = 1 - t;
            fadeOutImage.color = newColor;

            timer -= Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator SceneFadeOut(string sceneName) {
        float timer = fadeOutTime;

        while(timer > 0) {
            float t = 1 - (timer / fadeOutTime);

            Color newColor = fadeOutImage.color;
            newColor.a = t;
            fadeOutImage.color = newColor;

            timer -= Time.deltaTime;
            yield return null;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
