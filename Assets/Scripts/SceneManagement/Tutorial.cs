using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPrompt
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
        InStation,
        PickCar,
        PickStaff,
        SelectCar,
        Scored,
        AcceptDeal,
        LoseCar
    };
    public Condition condition;
    public bool completed;

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
        this.completed = false;

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
        ResetReferences();
        textTicket.SetActive(false);
        //spriteTicket.SetActive(false);
        Time.timeScale = 1;
        completed = true;
        tutorial.promptsToCheck.Remove(this);
        Debug.Log("Prompt should be removed: ");
        tutorial.PrintPrompts(tutorial.promptsToCheck);
        tutorial.textIndex++;
        tutorial.AddPrompts(new int []{tutorial.textIndex});
        Debug.Log("Next prompt should be added: ");
        tutorial.PrintPrompts(tutorial.promptsToCheck);
        tutorial.promptUp = false;
    }

    public bool ConditionCheck()
    {
        if (completed) 
        {
            Debug.LogError("Tutorial Prompt "+index+" has just been condition checked but is already completed.");
            tutorial.promptsToCheck.Remove(this);
            return false;
        }
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
                if (tutorial.inStation) tutorial.promptsToCheck.Add(tutorial.tutorialPrompts[10]);
                return tutorial.inStation;
            case Condition.PickCar:
                if (tutorial.carCard) 
                {
                    tutorial.AddPrompts(new int[] {11,12,13,14});
                }
                return tutorial.carCard;
            case Condition.PickStaff:
                return tutorial.staffCard;
            case Condition.SelectCar:
                GameObject trainObj = GameObject.Find("Train");
                if (trainObj)
                {
                    if (trainObj.GetComponent<Train>().SelectedCar > 0) return true;
                }
                return false;
            case Condition.Scored:
                return tutorial.scoringDone;
            case Condition.AcceptDeal:
                return tutorial.dealAccepted;
            case Condition.LoseCar:
                return tutorial.carLost;
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

    private GameObject trainObj;
    private Train trainTrain;
    private Task engineTask;

    public bool inStation = false;
    public bool waitDone = false;
    public bool taskSucceed = false;
    public bool carCard = false;
    public bool staffCard = false;
    public bool scoringDone = false;
    public bool dealAccepted = false;
    public bool carLost = false;

    public TutorialPrompt[] tutorialPrompts;
    public List<TutorialPrompt> promptsToCheck = new List<TutorialPrompt>();
    public GameObject defaultPositionObj;
    private Vector3 defaultPostition;

    public bool promptUp = false;

    public int textIndex = 0;
    //If the order of prompts changes then check these lines: 
    //  SelectCard() in CardChooser.cs line 68
    //  SucceedTask() in Task.cs line 99
    //  SelectCar() in Train.cs lines 110 and 115
    //  HideTicket() in Tutorial.cs lines 188, 189, 190

    void Start()
    {   
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
            Debug.Log("Created tutorial prompt "+i+" text: "+PopupTexts[i]+" Condition: "+condition);
        }

        StartCoroutine(DelayDisplayTicket(2));
        promptsToCheck.Add(tutorialPrompts[0]);
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
        if (tutorialActive) 
        {
            //Debug.Log(promptsToCheck);
            for (int i = 0; i < promptsToCheck.Count; i++)
            {
                if (!promptUp && promptsToCheck[i].ConditionCheck()) promptsToCheck[i].ShowTicket();
            }
        }   
    }


    public void HideTicket()
    {
        //In cases where HideTicket() is called in other places and shouldn't progress unless condtions are met
        if (textIndex == 2 && !engineTask.selected) return;
        if (textIndex == 3 && !taskSucceed) return;
        if (textIndex == 4 && engineTask.selected) return;
        
        tutorialPrompts[textIndex].HideTicket();  
        if (textIndex == tutorialPrompts.Length) tutorialActive = false; 
    }

    public void AddPrompts(int[] prompts)
    {
        foreach(int p in prompts)
        {
            if (!promptsToCheck.Contains(tutorialPrompts[p])) promptsToCheck.Add(tutorialPrompts[p]);
        }
    }

    public void PrintPrompts(List<TutorialPrompt> promptsToPrint)
    {
        foreach (TutorialPrompt p in promptsToPrint)
        {
            Debug.Log("    Prompt "+p.index+" with condition: "+p.condition+" is completed: "+p.completed);
        }
    }
}
