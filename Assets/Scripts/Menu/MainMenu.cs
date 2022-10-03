using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private GameObject settingsMenu, 
                       menuButtons, 
                       tutorial, 
                       overviewText, 
                       trainCarsText, 
                       trainOperationText,
                       staffText,
                       textBackground,
                       scoringAndWinningText;
    void Start()
    {
        settingsMenu = transform.Find("SettingsMenu").gameObject;
        tutorial = transform.Find("HowToPlay").gameObject;
        menuButtons = transform.Find("Buttons").gameObject;

        //Tutorial Descriptions
        overviewText = transform.Find("Overview Text").gameObject;
        trainOperationText = transform.Find("Train Cars Text").gameObject;
        trainCarsText = transform.Find("Train Operation Text").gameObject;
        staffText = transform.Find("Staff Text").gameObject;
        scoringAndWinningText = transform.Find("Winning Text").gameObject;
        textBackground = transform.Find("TextBackground").gameObject;

        //Hide tutorial text
        overviewText.SetActive(false);
        trainOperationText.SetActive(false);
        trainCarsText.SetActive(false);
        staffText.SetActive(false);
        scoringAndWinningText.SetActive(false);
        textBackground.SetActive(false);
    }

    public void ShowTutorial()
    {
        tutorial.SetActive(true);
        menuButtons.SetActive(false);
        settingsMenu.SetActive(false);
        overviewText.SetActive(true);
        textBackground.SetActive(true);
    }

    public void HideTutorial()
    {
        tutorial.SetActive(false);
        menuButtons.SetActive(true);
        settingsMenu.SetActive(true);
        overviewText.SetActive(false);
        textBackground.SetActive(false);
    }

    public void StartGame()
    {
        FindObjectOfType<Audio>().StartGame();
        SceneManager.LoadScene("trainplace", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowOverviewText()
    {
        overviewText.SetActive(true);
        trainOperationText.SetActive(false);
        trainCarsText.SetActive(false);
        staffText.SetActive(false);
        scoringAndWinningText.SetActive(false);
        textBackground.SetActive(true);
    }

    public void ShowTrainOperationText()
    {
        overviewText.SetActive(false);
        trainOperationText.SetActive(true);
        trainCarsText.SetActive(false);
        staffText.SetActive(false);
        scoringAndWinningText.SetActive(false);
        textBackground.SetActive(true);
    }

    public void ShowTrainCarText()
    {
        overviewText.SetActive(false);
        trainOperationText.SetActive(false);
        trainCarsText.SetActive(true);
        staffText.SetActive(false);
        scoringAndWinningText.SetActive(false);
        textBackground.SetActive(true);
    }

    public void ShowStaffText()
    {
        overviewText.SetActive(false);
        trainOperationText.SetActive(false);
        trainCarsText.SetActive(false);
        staffText.SetActive(true);
        scoringAndWinningText.SetActive(false);
        textBackground.SetActive(true);
    }

    public void ShowScoringAndWinningText()
    {
        overviewText.SetActive(false);
        trainOperationText.SetActive(false);
        trainCarsText.SetActive(false);
        staffText.SetActive(false);
        scoringAndWinningText.SetActive(true);
        textBackground.SetActive(true);
    }
}
