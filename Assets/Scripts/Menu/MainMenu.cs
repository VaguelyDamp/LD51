using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject settingsMenu;
    void Start()
    {
        settingsMenu = transform.Find("SettingsMenu").gameObject;
    }

    
    public void ShowSettings()
    {
        settingsMenu.SetActive(true);
    }

    public void HideSettings()
    {
        settingsMenu.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("station", LoadSceneMode.Single);

    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
