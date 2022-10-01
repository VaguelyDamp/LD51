using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDealer : MonoBehaviour
{
    public Transform cardRow;
    public Vector3 cardSpacing = new Vector3(250, 0, 0);
    
    private DeckManager deck;

    private HashSet<int> selectedCards;

    public int allowedSelection = 3;
    public int dealCount = 5;

    private GameObject[] dealtCards;

    public UnityEngine.UI.Button acceptButton;

    private void Awake() {
        selectedCards = new HashSet<int>();
        dealtCards = new GameObject[dealCount];
        deck = FindObjectOfType<DeckManager>();
    }

    private void Start() {
        SpawnCards();

        acceptButton.interactable = (selectedCards.Count == allowedSelection);

        
    }

    private void SpawnCards() {
        GameObject[] deal = deck.Draw(dealCount);
        Vector3 cardPos = ((dealCount - 1) / -2.0f) * cardSpacing;

        for(int i = 0; i < dealCount; ++i) {
            GameObject card = Instantiate(deal[i], cardRow);
            card.transform.localPosition = cardPos;
            card.GetComponent<CardChooser>().cardIndex = i;
            cardPos += cardSpacing;
            dealtCards[i] = deal[i];
        }
    }

    /*
        Returns a boolean for if the card should be selected after the operation
    */
    public bool RegisterCardSelection(int selection) {
        bool isSelected = false;
        if(selectedCards.Contains(selection)) {
            selectedCards.Remove(selection);
        }
        else {
            if (selectedCards.Count >= allowedSelection) {
            }
            else {
                selectedCards.Add(selection);
                isSelected = true;
            }
        }

        acceptButton.interactable = (selectedCards.Count == allowedSelection);
        return isSelected;
    }

    public void AcceptDeal() {
        if(selectedCards.Count == allowedSelection) {
            for (int i = 0; i < dealCount; ++i) {
                if (selectedCards.Contains(i)) {
                    deck.AddToHand(dealtCards[i]);
                }
                else {
                    // Return unchosen to deck
                    deck.AddToDeck(dealtCards[i]); 
                }
            }

            FindObjectOfType<SceneTransition>().StartSceneTransition();
        }
        else {
            // Rejection
        }
    }
}
