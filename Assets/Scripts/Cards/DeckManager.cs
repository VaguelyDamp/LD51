using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    public GameObject[] baseDeck;

    public List<GameObject> deck;

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
        int toDraw = Mathf.Min(count, deck.Count);
        GameObject[] deal = new GameObject[toDraw];
        for(int i = 0; i < toDraw; ++i) {
            deal[i] = Draw();
        }
        return deal;
    }

    public void AddToDeck(GameObject card) {
        deck.Add(card);
    }

    public GameObject AddToHand(GameObject card) {
        GameObject instance = Instantiate(card, transform);
        CardChooser cc = instance.GetComponent<CardChooser>();
        if(cc) {
            Destroy(cc);
        }
        CardDrag cd = instance.GetComponent<CardDrag>();
        if(cd) {
            Destroy(cd);
        }
        hand.Add(instance);

        return instance;
    }

    public int GetStaffSlotCountInHandByType(StaffCard.StaffType staffType) {
        int count = 0;

        foreach(GameObject card in hand) {
            CarCard cc = card.GetComponent<CarCard>();
            if(cc) {
                count += cc.GetSlotCountOfType(staffType);
            }
        }

        return count;
    }

    public int GetStaffCardCountInHandByType(StaffCard.StaffType staffType) {
        int count = 0;

        foreach(GameObject card in hand) {
            StaffCard sc = card.GetComponent<StaffCard>();
            if(sc) {
                if(sc.staffType == staffType) ++count;
            }
        }
        return count;
    }

    public int GetCarCardCountInHand() {
        int count = 0;

        foreach(GameObject card in hand) {
            CarCard cc = card.GetComponent<CarCard>();
            if(cc) {
                ++count;
            }
        }
        return count;
    }

    public List<GameObject> GetHand() {
        return hand;
    }
}
