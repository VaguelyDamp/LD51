using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public string[] PopupTexts;
    private GameObject textTicket;
    public GameObject spriteTicket;
    private GameObject trainObj;
    private Train trainTrain;
    private Task engineTask;
    private GameObject canvas;

    public bool inStation = false;

    public int textIndex = 0;
    void Start()
    {   
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        textTicket = canvas.transform.Find("TextTicket").gameObject;
        //spriteTicket = GameObject.FindGameObjectWithTag("SpriteTicket");
        trainObj = GameObject.FindGameObjectWithTag("Train");
        trainTrain = trainObj.GetComponent<Train>();
        engineTask = trainObj.transform.Find("Train Engine").gameObject.GetComponent<Task>();       

        trainTrain.initialSecondsToDestination = 10;
        trainTrain.secsToDest = 10;

        engineTask.minInterval = 100;
        engineTask.maxInterval = 100;

        StartCoroutine(DelayDisplayTicket(PopupTexts[0], 2));
    }

    private IEnumerator DelayDisplayTicket(string text, float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.Log("Done delay");
        DisplayTicket(text);
    }

    // Update is called once per frame
    void Update()
    {
        switch(textIndex)
        {
            case 1:
                DisplayTicket(PopupTexts[1]);
                break;
            case 2:
                engineTask.StartTask();
                DisplayTicket(PopupTexts[2]);
                break;
            case 3:
                if (engineTask.selected) DisplayTicket(PopupTexts[3]);
                break;
            case 4:
                if (!engineTask.taskUp) DisplayTicket(PopupTexts[4]);
                break;
            case 5:
                if (!engineTask.selected) DisplayTicket(PopupTexts[5]);
                break;
            case 6:
                DisplayTicket(PopupTexts[6]);
                break;
            case 7:
                if (inStation) DisplayTicket(PopupTexts[7]);
                break;
            case 8:
                DisplayTicket(PopupTexts[8]);
                break;
            case 9:
                DisplayTicket(PopupTexts[9]);
                break;
            case 10:
                DisplayTicket(PopupTexts[10]);
                break;
        }
    }

    public void DisplayTicket(string tuText)
    {
        canvas = GameObject.Find("Canvas");
        Debug.Log(canvas);
        textTicket = canvas.transform.Find("TextTicket").gameObject;
        Time.timeScale = 0;
        textTicket.SetActive(true);
        textTicket.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = tuText;
    }

    public void HideTicket()
    {
        canvas = GameObject.Find("Canvas");
        textTicket = canvas.transform.Find("TextTicket").gameObject;
        textTicket.SetActive(false);
        //spriteTicket.SetActive(false);
        Time.timeScale = 1;
        textIndex++;
    }
}
