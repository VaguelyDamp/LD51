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
                       starsTicket,
                       scoringAndWinningText;

    public GameObject dampBoiCard;
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

        //Find Stars
        starsTicket = transform.Find("StarsTicket").gameObject;

        starsTicket.transform.Find("HighScore").Find("Num").GetComponent<TMPro.TextMeshProUGUI>().text = PlayerPrefs.GetInt("HighScore", 0).ToString();
        starsTicket.transform.Find("TotalStars").Find("Num").GetComponent<TMPro.TextMeshProUGUI>().text = PlayerPrefs.GetInt("TotalStars", 0).ToString();
        starsTicket.transform.Find("BuyButton").GetComponent<UnityEngine.UI.Button>().interactable = PlayerPrefs.GetInt("TotalStars", 0) > 50;
    }

    public void BuyDampBoi()
    {
        int totalStars = PlayerPrefs.GetInt("TotalStars", 0);
        if (totalStars >= 50)
        {
            totalStars -= 50;
            PlayerPrefs.SetInt(dampBoiCard.name, PlayerPrefs.GetInt(dampBoiCard.name, 0) + 1);
            PlayerPrefs.SetInt("TotalStars", totalStars);
            starsTicket.transform.Find("TotalStars").Find("Num").GetComponent<TMPro.TextMeshProUGUI>().text = totalStars.ToString();
            starsTicket.transform.Find("BuyButton").GetComponent<UnityEngine.UI.Button>().interactable = PlayerPrefs.GetInt("TotalStars", 0) > 50;
        }
    }

    public void ShowTutorial()
    {
        tutorial.SetActive(true);
        menuButtons.SetActive(false);
        settingsMenu.SetActive(false);
        overviewText.SetActive(true);
        textBackground.SetActive(true);
        starsTicket.SetActive(false);
    }

    public void HideTutorial()
    {
        tutorial.SetActive(false);
        menuButtons.SetActive(true);
        settingsMenu.SetActive(true);
        overviewText.SetActive(false);
        textBackground.SetActive(false);
        starsTicket.SetActive(true);
    }

    public void StartGame()
    {
        FindObjectOfType<Audio>().StartGame();
        if (DeckManager.instance)
        {
            DeckManager.instance.ResetDeck();
            DeckManager.instance.LoadDeck();
        }       
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
        starsTicket.SetActive(false);
    }

    public void ShowTrainOperationText()
    {
        overviewText.SetActive(false);
        trainOperationText.SetActive(true);
        trainCarsText.SetActive(false);
        staffText.SetActive(false);
        scoringAndWinningText.SetActive(false);
        textBackground.SetActive(true);
        starsTicket.SetActive(false);
    }

    public void ShowTrainCarText()
    {
        overviewText.SetActive(false);
        trainOperationText.SetActive(false);
        trainCarsText.SetActive(true);
        staffText.SetActive(false);
        scoringAndWinningText.SetActive(false);
        textBackground.SetActive(true);
        starsTicket.SetActive(false);
    }

    public void ShowStaffText()
    {
        overviewText.SetActive(false);
        trainOperationText.SetActive(false);
        trainCarsText.SetActive(false);
        staffText.SetActive(true);
        scoringAndWinningText.SetActive(false);
        textBackground.SetActive(true);
        starsTicket.SetActive(false);
    }

    public void ShowScoringAndWinningText()
    {
        overviewText.SetActive(false);
        trainOperationText.SetActive(false);
        trainCarsText.SetActive(false);
        staffText.SetActive(false);
        scoringAndWinningText.SetActive(true);
        textBackground.SetActive(true);
        starsTicket.SetActive(false);
    }
}
