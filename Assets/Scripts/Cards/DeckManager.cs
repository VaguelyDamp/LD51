using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject[] baseDeck;

    private List<GameObject> deck;

    public List<GameObject> hand;

    private void Awake() {
        if(FindObjectOfType<DeckManager>() != this) {
            Destroy(gameObject);
        }

        deck = new List<GameObject>();
        deck.AddRange(baseDeck);

        DontDestroyOnLoad(gameObject);
    }

    public GameObject Draw() {
        int index = Random.Range(0, deck.Count);
        GameObject chosen = deck[index];
        deck.RemoveAt(index);
        return chosen;
    }

    public GameObject[] Draw(int count) {
        GameObject[] deal = new GameObject[count];
        for(int i = 0; i < count; ++i) {
            deal[i] = Draw();
        }
        return deal;
    }

    public void AddToDeck(GameObject card) {
        deck.Add(card);
    }

    public void AddToHand(GameObject card) {
        hand.Add(card);
    }

    public List<GameObject> GetHand() {
        return hand;
    }
}
