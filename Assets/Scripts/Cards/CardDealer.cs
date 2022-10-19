using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CardDealer : MonoBehaviour
{
    public Transform cardRow;
    public Vector3 cardSpacing = new Vector3(250, 0, 0);

    private HashSet<int> selectedCards;

    public int minAllowedSelection = 3;
    public int maxAllowedSelection = 3;
    public int dealCount = 5;

    private GameObject[] dealtCards;
    private GameObject[] dealtCardObjs;

    public UnityEngine.UI.Button acceptButton;

    public GameObject TrainPrefab;

    public Transform TrainSpawnPosition;
    public Transform TrainWaitPosition;

    public float trainApproachTime;

    public bool choosingCards;
    public bool assigningStaff;

    private int staffToAssign;

    private Vector3 selectedCardShoop = new Vector3(-2000, 1000, 0);
    private Vector3 discardedCardShoop = new Vector3(0, -2000, 0);

    public TMPro.TextMeshProUGUI janitorVacancyUi;
    public TMPro.TextMeshProUGUI engineerVacancyUi;
    public TMPro.TextMeshProUGUI cookVacancyUi;
    public TMPro.TextMeshProUGUI conductorVacancyUi;

    public AudioClip dragBoop;
    public AudioClip dropBoop;

    public AudioClip readyBoop;

    private AudioSource sauce;
    private int nonDampBois = 0;

    private void Awake() {
        selectedCards = new HashSet<int>();
        staffToAssign = 0;
    }

    private void Start() {
        sauce = GetComponent<AudioSource>();
        if (DeckManager.instance.stationIndex == 0) actualStart();
        else FindObjectOfType<Scoring>().StartScoring();    

        acceptButton.interactable = false;    
    }

    public void actualStart()
    {
        SpawnCards();

        acceptButton.interactable = (selectedCards.Count >= minAllowedSelection);

        choosingCards = true;
        assigningStaff = false;

        UpdateJobVacanciesUI();

        StartCoroutine(BringInTrain());
    }

    private void SpawnCards() {
        dealtCards = DeckManager.instance.Draw(dealCount);
        dealtCardObjs = new GameObject[dealtCards.Length];

        Vector3 cardPos = ((dealtCards.Length - 1) / -2.0f) * cardSpacing;

        for(int i = 0; i < dealtCards.Length; ++i) {
            GameObject card = Instantiate(dealtCards[i], cardRow);
            card.transform.localPosition = cardPos;
            card.GetComponent<CardChooser>().cardIndex = i;
            cardPos += cardSpacing;

            dealtCardObjs[i] = card;
        }
    }

    private int GetAddedStaffSlotsWithCurrentSelection(StaffCard.StaffType staffType) {
        int count = 0;
        foreach(int selected in selectedCards) {
            CarCard cc = dealtCards[selected].GetComponent<CarCard>();
            if(cc) {
                if (staffType == StaffCard.StaffType.DampBoi) count += cc.staffSlots.Length;
                else count += cc.GetSlotCountOfType(staffType);
            }
        }
        return count;
    }

    private int GetAddedStaffCardsWithCurrentSelection(StaffCard.StaffType staffType, bool countDamp = true) {
        int count = 0;
        foreach(int selected in selectedCards) {
            StaffCard sc = dealtCards[selected].GetComponent<StaffCard>();
            if(sc && (sc.staffType == staffType || (sc.staffType == StaffCard.StaffType.DampBoi && countDamp))) {
                ++count;
            }
        }
        return count;
    }

    private void UpdateJobVacanciesUI() {
        int numJanitors = CalculateVacanciesForJobType(StaffCard.StaffType.Janitor);
        int numEngineers = CalculateVacanciesForJobType(StaffCard.StaffType.Engineer);
        int numCooks = CalculateVacanciesForJobType(StaffCard.StaffType.Cook);
        int numConductors = CalculateVacanciesForJobType(StaffCard.StaffType.Conductor);
        
        int numDampBois = CountSelectedDampBois();  

        janitorVacancyUi.text = (numJanitors + numDampBois).ToString() + (numDampBois > 0 ? "*" : "");
        engineerVacancyUi.text = (numEngineers + numDampBois).ToString() + (numDampBois > 0 ? "*" : "");
        cookVacancyUi.text = (numCooks + numDampBois).ToString() + (numDampBois > 0 ? "*" : "");
        conductorVacancyUi.text = (numConductors + numDampBois).ToString() + (numDampBois > 0 ? "*" : "");
    }

    private int CalculateVacanciesForJobType(StaffCard.StaffType staffType, bool countDamp = true) {
        int currentSlots = DeckManager.instance.GetStaffSlotCountInHandByType(staffType);
        int currentStaff = DeckManager.instance.GetStaffCardCountInHandByType(staffType);

        int addedSlots = GetAddedStaffSlotsWithCurrentSelection(staffType);
        int addedStaff = GetAddedStaffCardsWithCurrentSelection(staffType, countDamp);

        int totalSlots = currentSlots + addedSlots;
        int totalStaff = currentStaff + addedStaff;

        return totalSlots - totalStaff;
    }

    private int CalculateAllJobVacancies()
    //Calculates all job vacancies not counting damp bois that are selected
    {
        int vacancies = 0;
        List<StaffCard.StaffType> allStaffTypes = new List<StaffCard.StaffType>() {StaffCard.StaffType.Janitor, StaffCard.StaffType.Engineer, StaffCard.StaffType.Conductor, StaffCard.StaffType.Cook};
        foreach (StaffCard.StaffType st in allStaffTypes)
        {
            vacancies += CalculateVacanciesForJobType(st, false); 
        } 
        return vacancies;
    }

    private int CountSelectedDampBois()
    {
        int numDampBois = 0;
        foreach(int selected in selectedCards) {
            StaffCard sc = dealtCards[selected].GetComponent<StaffCard>();
            if(sc) {
                if (sc.staffType == StaffCard.StaffType.DampBoi) numDampBois++;
            }
        }
        return numDampBois;
    }

    private int SelectedCarCardCount() {
        int count = 0;
        foreach(int selected in selectedCards) {
            CarCard cc = dealtCards[selected].GetComponent<CarCard>();
            if(cc) {
                count++;
            }
        }
        return count;
    }

    /*
        Returns a boolean for if the card should be selected after the operation
    */
    public bool RegisterCardSelection(int selection) {
        if(!choosingCards) return true;

        

        bool isSelected = false;
        if(selectedCards.Contains(selection)) {
            CarCard car = dealtCards[selection].GetComponent<CarCard>();
            if(car) {
                foreach(var slotType in car.staffSlots) {
                    int curVacancies = CalculateVacanciesForJobType(slotType);
                    int slotCountChange = car.GetSlotCountOfType(slotType);

                    if(curVacancies - slotCountChange < 0) {
                        isSelected = true;
                        break;
                    }
                }
                if(!isSelected) {
                    selectedCards.Remove(selection);
                }
            }
            else {
                selectedCards.Remove(selection);
            }
        }
        else {
            if (selectedCards.Count >= maxAllowedSelection) {
            }
            else {
                StaffCard staff = dealtCards[selection].GetComponent<StaffCard>();
                if(staff) {
                    var staffType = staff.staffType;
                    int curVacancies = CalculateVacanciesForJobType(staffType);

                    if (CountSelectedDampBois() > 0)
                    {    
                        int curVacanciesSansDamp = CalculateVacanciesForJobType(staffType, false);
                        if(curVacanciesSansDamp > Mathf.Max(0, curVacancies))
                        {
                            if ((CalculateAllJobVacancies() > CountSelectedDampBois()))
                            {
                                selectedCards.Add(selection);
                                isSelected = true;
                            }   
                        }
                    }
                    else if (curVacancies > 0)
                    {
                        selectedCards.Add(selection);
                        isSelected = true;
                    }

                }
                else {
                    int currentTrainLength = DeckManager.instance.GetCarCardCountInHand();
                    int addedTrainLength = SelectedCarCardCount();

                    if(currentTrainLength + addedTrainLength < 9) {
                        selectedCards.Add(selection);
                        isSelected = true;
                    }
                }
            }
        }

        acceptButton.interactable = (selectedCards.Count >= minAllowedSelection);
        UpdateJobVacanciesUI();
        return isSelected;
    }

    private IEnumerator DropOutCard(GameObject card, Vector3 motion) {
        float timer = 0;

        Vector3 initialScale = card.transform.localScale;

        Debug.LogFormat("Shoopin card {0} which is a child of {1}", card.name, card.transform.parent.name);

        CardChooser cc = card.GetComponent<CardChooser>();
        if(cc) Destroy(cc);
        CardDrag cd = card.GetComponent<CardDrag>();
        if(cd) Destroy(cd);

        while(timer < 1) {
            timer += Time.deltaTime;

            card.transform.position += motion * Time.deltaTime;
            card.transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, timer);
            
            yield return null;
        }

        Destroy(card);
    }

    public void MarkStaffCardAssigned(GameObject staffCard) {
        staffToAssign--;
        Tutorial tutorial = DeckManager.instance.GetComponent<Tutorial>();
        if (tutorial) 
        {
            tutorial.staffAssigned = true;
            tutorial.HideTicket();
        }      

        if (staffCard.GetComponent<StaffCard>().staffType != StaffCard.StaffType.DampBoi) nonDampBois--;

        sauce.PlayOneShot(dropBoop);

        StartCoroutine(DropOutCard(staffCard, selectedCardShoop));

        CheckDampEnable();

        if(staffToAssign <= 0) {
            sauce.PlayOneShot(readyBoop);
            FindObjectOfType<SceneTransition>().StartSceneTransition();
            StartCoroutine(SendOutTrain());
        }
    }

    public void CheckDampEnable()
    {
        if (nonDampBois <= 0)
        {
            foreach (Transform card in transform.Find("CardRow"))
            {
                if (card.GetComponent<CardDrag>()) card.GetComponent<CardDrag>().draggable = true;
                card.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            }
        }  
    }

    public void AcceptDeal() {
        if(choosingCards && selectedCards.Count >= minAllowedSelection && selectedCards.Count <= maxAllowedSelection) { //Can accept deal
            staffToAssign = 0;
            nonDampBois = 0;
            choosingCards = false;
            Tutorial tutorial = DeckManager.instance.GetComponent<Tutorial>();
            for (int i = 0; i < dealCount; ++i) {
                //Check all dealt cards
                if (selectedCards.Contains(i)) {
                    if(dealtCards[i].GetComponent<StaffCard>()) {
                        //Staff cards that need to be assigned
                        CardDrag cd = dealtCardObjs[i].AddComponent<CardDrag>();
                        cd.dragBoop = dragBoop;

                        CardChooser cc = dealtCardObjs[i].GetComponent<CardChooser>();
                        cc.Selected = true;
                        cc.GetComponent<UnityEngine.UI.Button>().enabled = false;
                        staffToAssign++;
                        if (tutorial) tutorial.assigneStaff = true;
                        if (dealtCards[i].GetComponent<StaffCard>().staffType != StaffCard.StaffType.DampBoi)
                        {
                            nonDampBois++;
                        }
                    }
                    else {
                        //Add car cards to hand
                        StartCoroutine(DropOutCard(dealtCardObjs[i], selectedCardShoop));
                        dealtCardObjs[i] = null;
                        DeckManager.instance.AddToHand(dealtCards[i]);
                    }
                }
                else {
                    // Return unchosen to deck
                    DeckManager.instance.AddToDeck(dealtCards[i]); 
                    StartCoroutine(DropOutCard(dealtCardObjs[i], discardedCardShoop));
                    dealtCardObjs[i] = null;
                }
            }

            Destroy(acceptButton.gameObject);

            sauce.PlayOneShot(readyBoop);

            
            if (tutorial) tutorial.dealAccepted = true;

            if(staffToAssign > 0) {
                if (nonDampBois == 0)
                {
                    Debug.Log("All Damp Bois");
                    foreach (Transform card in transform.Find("CardRow"))
                    {
                        Debug.Log("Card Row:"+card.name); 
                        if (card.GetComponent<CardDrag>()) card.GetComponent<CardDrag>().draggable = true;
                        card.GetComponent<UnityEngine.UI.Image>().color = Color.white;
                    }
                }
                FindObjectOfType<UI_Train>().SpawnTrain();
            }
            else {
                FindObjectOfType<SceneTransition>().StartSceneTransition();
                StartCoroutine(SendOutTrain());
            }            
        }
        else {
            // Rejection
            Debug.Log("No Sex 4 u");
        }
    }

    private IEnumerator BringInTrain() {
        GameObject train = Instantiate(TrainPrefab, TrainSpawnPosition.position, TrainSpawnPosition.rotation);
        Train traintrain = train.GetComponent<Train>();

        traintrain.CurrentSpeed = 0;
        traintrain.disableLogic = true;

        float moveTimer = trainApproachTime;

        while(moveTimer > 0) {
            float t = 1 - (moveTimer / trainApproachTime);

            if (t <= 0.5f) if (DeckManager.instance.gameObject.GetComponent<Tutorial>() != null) DeckManager.instance.gameObject.GetComponent<Tutorial>().inStation = true;

            train.transform.position = Vector3.Lerp(TrainSpawnPosition.position, TrainWaitPosition.position, EasingFunction.EaseOutCubic(0, 1, t));

            moveTimer -= Time.deltaTime;
            yield return null;
        }       
    }

    private IEnumerator SendOutTrain() {
        GameObject train = FindObjectOfType<Train>().gameObject;
        Train traintrain = train.GetComponent<Train>();

        traintrain.CurrentSpeed = 0;
        traintrain.disableLogic = true;

        Vector3 endPos = TrainWaitPosition.position + (TrainWaitPosition.position - TrainSpawnPosition.position);

        float moveTimer = trainApproachTime;

        while(moveTimer > 0) {
            float t = 1 - (moveTimer / trainApproachTime);

            train.transform.position = Vector3.Lerp(TrainWaitPosition.position, endPos, EasingFunction.EaseInCubic(0, 1, t));

            moveTimer -= Time.deltaTime;
            yield return null;
        }

    }

    public void TutorialButton()
    {
        DeckManager.instance.gameObject.GetComponent<Tutorial>().HideTicket();
    }
}
