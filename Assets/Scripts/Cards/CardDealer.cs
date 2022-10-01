using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDealer : MonoBehaviour
{
    public GameObject[] cardPrefabs;
    public Transform cardRow;
    public Vector3 cardSpacing = new Vector3(250, 0, 0);

    private HashSet<int> selectedCards;

    public int allowedSelection = 3;
    public int dealCount = 5;

    private GameObject[] dealtCards;

    private void Start() {
        selectedCards = new HashSet<int>();
        dealtCards = new GameObject[dealCount];

        SpawnCards();
    }

    private void SpawnCards() {
        Vector3 cardPos = ((dealCount - 1) / -2.0f) * cardSpacing;

        for(int i = 0; i < dealCount; ++i) {
            GameObject card = Instantiate(cardPrefabs[Random.Range(0, cardPrefabs.Length)], cardRow);
            card.transform.localPosition = cardPos;
            card.GetComponent<CardChooser>().cardIndex = i;
            cardPos += cardSpacing;
        }
    }

    /*
        Returns a boolean for if the card should be selected after the operation
    */
    public bool RegisterCardSelection(int selection) {
        if(selectedCards.Contains(selection)) {
            selectedCards.Remove(selection);
            Debug.LogFormat("Removed {0}", selection);
            return false;
        }
        else {
            if (selectedCards.Count >= allowedSelection) {
                Debug.LogFormat("Selection full, rejected {0}", selection);
                return false;
            }
            else {
                selectedCards.Add(selection);
                Debug.LogFormat("Selected {0}", selection);
                return true;
            }
        }
        
    }
}
