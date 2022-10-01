using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public string[] scenesToLoad;
    public string[] scenesRequiredToLoad;

    void Awake()
    {
        foreach(string sceneName in scenesRequiredToLoad)
        {
            SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        }

        foreach(string sceneName in scenesToLoad)
        {
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        }
    }
}