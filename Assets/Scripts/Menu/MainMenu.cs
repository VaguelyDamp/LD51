using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject settingsMenu, menuButtons, tutorial;
    void Start()
    {
        settingsMenu = transform.Find("SettingsMenu").gameObject;
        tutorial = transform.Find("HowToPlay").gameObject;
        menuButtons = transform.Find("Buttons").gameObject;
    }

    public void ShowTutorial()
    {
        tutorial.SetActive(true);
        menuButtons.SetActive(false);
        settingsMenu.SetActive(false);
    }

    public void HideTutorial()
    {
        tutorial.SetActive(false);
        menuButtons.SetActive(true);
        settingsMenu.SetActive(true);
    }

    public void StartGame()
    {
        FindObjectOfType<Audio>().StartGame();
        SceneManager.LoadScene("station", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
