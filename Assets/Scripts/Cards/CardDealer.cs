using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDealer : MonoBehaviour
{
    public Transform cardRow;
    public Vector3 cardSpacing = new Vector3(250, 0, 0);
    
    private DeckManager deck;

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

    private void Awake() {
        selectedCards = new HashSet<int>();
        deck = FindObjectOfType<DeckManager>();
        staffToAssign = 0;
    }

    private void Start() {
        SpawnCards();

        acceptButton.interactable = (selectedCards.Count >= minAllowedSelection);

        choosingCards = true;
        assigningStaff = false;

        StartCoroutine(BringInTrain());
    }

    private void SpawnCards() {
        dealtCards = deck.Draw(dealCount);
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

    /*
        Returns a boolean for if the card should be selected after the operation
    */
    public bool RegisterCardSelection(int selection) {
        if(!choosingCards) return true;

        bool isSelected = false;
        if(selectedCards.Contains(selection)) {
            selectedCards.Remove(selection);
        }
        else {
            if (selectedCards.Count >= maxAllowedSelection) {
            }
            else {
                selectedCards.Add(selection);
                isSelected = true;
            }
        }

        acceptButton.interactable = (selectedCards.Count >= minAllowedSelection);
        return isSelected;
    }

    private IEnumerator DropOutCard(GameObject card, Vector3 motion) {
        float timer = 0;

        card.GetComponent<CardChooser>().enabled = false;
        card.GetComponent<CardDrag>().enabled = false;

        while(timer < 1) {
            timer += Time.deltaTime;

            card.transform.position += motion * Time.deltaTime;
            yield return null;
        }

        Destroy(card);
    }

    public void MarkStaffCardAssigned(GameObject staffCard) {
        staffToAssign--;

        DropOutCard(staffCard, Vector3.left * 2000);

        if(staffToAssign <= 0) {
            FindObjectOfType<SceneTransition>().StartSceneTransition();
            StartCoroutine(SendOutTrain());
        }
    }

    public void AcceptDeal() {
        if(choosingCards && selectedCards.Count >= minAllowedSelection && selectedCards.Count <= maxAllowedSelection) {
            staffToAssign = 0;
            choosingCards = false;
            for (int i = 0; i < dealCount; ++i) {
                if (selectedCards.Contains(i)) {
                    if(dealtCards[i].GetComponent<StaffCard>()) {
                        dealtCardObjs[i].AddComponent<CardDrag>();
                        CardChooser cc = dealtCardObjs[i].GetComponent<CardChooser>();
                        cc.Selected = true;
                        cc.GetComponent<UnityEngine.UI.Button>().enabled = false;
                        staffToAssign++;
                    }
                    else {
                        StartCoroutine(DropOutCard(dealtCardObjs[i], Vector3.left * 2000));
                        dealtCardObjs[i] = null;
                        deck.AddToHand(dealtCards[i]);
                    }
                }
                else {
                    // Return unchosen to deck
                    deck.AddToDeck(dealtCards[i]); 
                    StartCoroutine(DropOutCard(dealtCardObjs[i], Vector3.down * 1000));
                    dealtCardObjs[i] = null;
                }
            }

            Destroy(acceptButton.gameObject);

            

            if(staffToAssign > 0) {
                FindObjectOfType<UI_Train>().SpawnTrain();
            }
            else {
                FindObjectOfType<SceneTransition>().StartSceneTransition();
                StartCoroutine(SendOutTrain());
            }            
        }
        else {
            // Rejection
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
}
