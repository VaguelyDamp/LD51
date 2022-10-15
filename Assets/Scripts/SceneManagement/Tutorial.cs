using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TutorialPrompt
{
    private Tutorial tutorial;

    public string text;
    public int index;
    public Vector3 ticketPosition;
    public enum Condition 
    { 
        None,
        Wait,
        EngineTaskSel,
        EngineTaskDesel,
        EngineTaskUp,
        EngineTaskSuccess,
        InStation
    };
    public Condition condition;

    private GameObject canvas;
    private GameObject textTicket;
    private Task engineTask;
    


    public TutorialPrompt(Tutorial tutorial, string text, int index, Vector3 ticketPosition, Condition condition = Condition.None)
    {
        this.tutorial = tutorial;
        this.text = text;
        this.index = index;
        this.ticketPosition = ticketPosition;
        this.condition = condition;

        this.canvas = GameObject.Find("Canvas");
        this.textTicket = canvas.transform.Find("TextTicket").gameObject;
        engineTask = GameObject.FindGameObjectWithTag("Train").transform.Find("Train Engine").gameObject.GetComponent<Task>();
    }
    
    public void ShowTicket()
    {
        ResetReferences();
        Debug.Log("Tutorial "+index+" text: "+text+" condition: "+condition);
        tutorial.promptUp = true;
        Time.timeScale = 0;
        textTicket.SetActive(true);
        textTicket.transform.position = ticketPosition;
        textTicket.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = text;

        if (index == 2) engineTask.StartTask();
    }

    public void ResetReferences()
    {
        canvas = GameObject.Find("Canvas");
        textTicket = canvas.transform.Find("TextTicket").gameObject;
    }

    public void HideTicket()
    {
        textTicket.SetActive(false);
        //spriteTicket.SetActive(false);
        Time.timeScale = 1;
        tutorial.textIndex++;
        tutorial.promptUp = false;
    }

    public bool ConditionCheck()
    {
        switch(condition)
        {
            case Condition.None:
                return true;
            case Condition.Wait:
                return tutorial.waitDone;
            case Condition.EngineTaskSel:
                return engineTask.selected;
            case Condition.EngineTaskDesel:
                return !engineTask.selected;
            case Condition.EngineTaskUp:
                return engineTask.taskUp;
            case Condition.EngineTaskSuccess:
                return tutorial.taskSucceed;
            case Condition.InStation:
                ResetReferences();
                return tutorial.inStation;
            default:
                return true;
        }
    }
}

public class Tutorial : MonoBehaviour
{
    public bool tutorialActive = false;

    public string[] PopupTexts;
    public TutorialPrompt.Condition[] conditions;

    private GameObject textTicket;
    public GameObject spriteTicket;
    private GameObject trainObj;
    private Train trainTrain;
    private Task engineTask;
    private GameObject canvas;

    public bool inStation = false;
    public bool waitDone = false;
    public bool taskSucceed = false;

    public TutorialPrompt[] tutorialPrompts;
    public GameObject defaultPositionObj;
    private Vector3 defaultPostition;

    public bool promptUp = false;

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

        if (defaultPositionObj) defaultPostition = defaultPositionObj.transform.position;
        else defaultPostition = new Vector3();

        if (PopupTexts.Length != conditions.Length) Debug.LogError("PopupTexts is not the same length as conditions! Enusre that both arrays are the same lenght.");

        tutorialPrompts =  new TutorialPrompt[PopupTexts.Length];
        for (int i=0; i < PopupTexts.Length; i++)
        {
            TutorialPrompt.Condition condition = TutorialPrompt.Condition.None;
            if (i < conditions.Length) condition = conditions[i];
            tutorialPrompts[i] = new TutorialPrompt(this, PopupTexts[i], i, defaultPostition, condition);
            Debug.Log("Created tutorial prompt "+i+" text: "+PopupTexts[i]);
        }

        StartCoroutine(DelayDisplayTicket(2));
        tutorialActive = true;
    }

    private IEnumerator DelayDisplayTicket(float delay)
    {
        waitDone = false;
        yield return new WaitForSeconds(delay);
        waitDone = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialActive) if (!promptUp && tutorialPrompts[textIndex].ConditionCheck()) tutorialPrompts[textIndex].ShowTicket();


        // switch(textIndex)
        // {
        //     case 1:
        //         DisplayTicket(PopupTexts[1]);
        //         break;
        //     case 2:
        //         engineTask.StartTask();
        //         DisplayTicket(PopupTexts[2]);
        //         break;
        //     case 3:
        //         if (engineTask.selected) DisplayTicket(PopupTexts[3]);
        //         break;
        //     case 4:
        //         if (!engineTask.taskUp) DisplayTicket(PopupTexts[4]);
        //         break;
        //     case 5:
        //         if (!engineTask.selected) DisplayTicket(PopupTexts[5]);
        //         break;
        //     case 6:
        //         DisplayTicket(PopupTexts[6]);
        //         break;
        //     case 7:
        //         if (inStation) DisplayTicket(PopupTexts[7]);
        //         break;
        //     case 8:
        //         DisplayTicket(PopupTexts[8]);
        //         break;
        //     case 9:
        //         DisplayTicket(PopupTexts[9]);
        //         break;
        //     case 10:
        //         DisplayTicket(PopupTexts[10]);
        //         break;
        // }
    }

    // public void DisplayTicket(string tuText)
    // {
    //     canvas = GameObject.Find("Canvas");
    //     //Debug.Log(canvas);
    //     textTicket = canvas.transform.Find("TextTicket").gameObject;
    //     Time.timeScale = 0;
    //     textTicket.SetActive(true);
    //     textTicket.transform.Find("Text").GetComponent<TMPro.TextMeshProUGUI>().text = tuText;
    // }

    public void HideTicket()
    {
        if (textIndex == 2 && !engineTask.selected) return;
        if (textIndex == 3 && !taskSucceed) return;
        if (textIndex == 4 && engineTask.selected) return;
        tutorialPrompts[textIndex].HideTicket();  
        if (textIndex == tutorialPrompts.Length) tutorialActive = false; 
    }
}
