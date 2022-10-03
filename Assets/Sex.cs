using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sex : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadMenuScene());
    }

    private IEnumerator LoadMenuScene()
    {
        yield return new WaitForSeconds(11);
        SceneManager.LoadScene("MainMenu");
    }
}