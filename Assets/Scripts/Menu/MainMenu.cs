using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject settingsMenu, menuButtons;
    void Start()
    {
        settingsMenu = transform.Find("SettingsMenu").gameObject;
        settingsMenu.SetActive(false);
        menuButtons = transform.Find("Buttons").gameObject;
    }

    
    public void ShowSettings()
    {
        settingsMenu.SetActive(true);
        menuButtons.SetActive(false); 
    }

    public void HideSettings()
    {
        settingsMenu.SetActive(false);
        menuButtons.SetActive(true);
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
